using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;

public class Matchmaking : MonoBehaviour
{
    public static Matchmaking instance;

    private string queueName;
    private string ticketId;
    private Coroutine pollTicket = null;
    private Action<GetMatchResult> matchingSuccessCallback;
    private Action<PlayFabError> matchingErrorCallback;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void FindMatch(string queue, Action<GetMatchResult> successCallback = null, Action<PlayFabError> errorCallback = null)
    {
        matchingSuccessCallback = successCallback;
        matchingErrorCallback = errorCallback;

        if (!Authentification.instance.isAuthenticated)
            return;

        switch (queue)
        {
            case Queues.OneVsOne:
                FindOneVsOneMatch();
                break;
        }
    }

    public void CancelMatchmaking(string queue)
    {
        StopPollTicket();

        PlayFabMultiplayerAPI.CancelAllMatchmakingTicketsForPlayer(
            new CancelAllMatchmakingTicketsForPlayerRequest
            {
                Entity = new EntityKey
                {
                    Id = Authentification.instance.userInfo.entityId,
                    Type = "title_player_account"
                },
                QueueName = queue
            },
            OnCancelMatchmaking,
            OnMatchmakingError
            );
    }

    private void FindOneVsOneMatch()
    {
        // DEBUG
        Debug.Log("Entering matchmaking.");

        queueName = Queues.OneVsOne;

        PlayFabMultiplayerAPI.CreateMatchmakingTicket(
            new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new EntityKey
                    {
                        Id = Authentification.instance.userInfo.entityId,
                        Type = "title_player_account"
                    },
                    Attributes = new MatchmakingPlayerAttributes
                    {
                        DataObject = new
                        {
                            Latencies = new object[]
                               {
                                   new {
                                       region = "EastUs",
                                       latency = 50
                                   }
                               }
                        }
                    }
                },
                QueueName = queueName,
                GiveUpAfterSeconds = 60
            },
            OnMatchmakingTicketCreated,
            OnMatchmakingError
            );
    }

    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
    {
        // DEBUG
        Debug.Log("In matchmaking (ticketId = " + result.TicketId + ").");

        ticketId = result.TicketId;
        pollTicket = StartCoroutine(PollTicket());
    }

    private IEnumerator PollTicket()
    {
        while (true)
        {
            PlayFabMultiplayerAPI.GetMatchmakingTicket(new GetMatchmakingTicketRequest
            {
                TicketId = ticketId,
                QueueName = queueName
            },
            OnGetMatchmakingTicket,
            OnMatchmakingError
            );
            yield return new WaitForSeconds(6);
        }
    }

    private void OnGetMatchmakingTicket(GetMatchmakingTicketResult result)
    {
        switch (result.Status)
        {
            case "Matched":
                StopPollTicket();
                StartMatch(result.MatchId);
                break;
            case "Canceled":
                StopPollTicket();
                break;

            // DEBUG
            default:
                Debug.Log(result.Status);
                break;
        }
    }

    private void StartMatch(string matchId)
    {
        // DEBUG
        Debug.Log("Match found.");

        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = queueName
            },
            OnGetMatch,
            OnMatchmakingError
            );
    }

    private void OnGetMatch(GetMatchResult result)
    {
        // DEBUG
        //Debug.Log("Server ip : " + result.ServerDetails.IPV4Address);
        //Debug.Log("Server port : " + result.ServerDetails.Ports[0].Num);

        matchingSuccessCallback?.Invoke(result);
    }

    private void OnCancelMatchmaking(CancelAllMatchmakingTicketsForPlayerResult result)
    {
        // DEBUG
        Debug.Log("Matchmaking canceled.");
    }

    private void StopPollTicket()
    {
        if (pollTicket == null)
            return;
        StopCoroutine(pollTicket);
        pollTicket = null;
    }

    private void OnMatchmakingError(PlayFabError error)
    {
        // DEBUG
        Debug.Log("Matchmaking error : " + error.ErrorMessage + ".");
        foreach (KeyValuePair<string, List<string>> entry in error.ErrorDetails)
        {
            Debug.Log(entry.Key);
            foreach (string message in entry.Value)
                Debug.Log(message);
        }

        matchingErrorCallback?.Invoke(error);
    }
}
