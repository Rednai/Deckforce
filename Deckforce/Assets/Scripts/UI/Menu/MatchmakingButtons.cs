using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.MultiplayerModels;

public class MatchmakingButtons : MonoBehaviour
{
    public Button findMatchButton;
    public Button cancelMatchButton;
    public Text findMatchText;
    public Text errorText;
    public GameObject waitingForPlayers;
    public PlayersSelection playerSelection;

    public void OnFindMatch()
    {
        findMatchText.text = "Finding a match...";
        findMatchButton.interactable = false;
        cancelMatchButton.interactable = true;
        Matchmaking.instance.FindMatch(Queues.OneVsOne, OnMatchFound);
    }

    
    public void OnCancelMatch()
    {
        Matchmaking.instance.CancelMatchmaking(Queues.OneVsOne);
        ResetButtons();
    }

    private void ResetButtons()
    {
        findMatchText.text = "Find a match";
        findMatchButton.interactable = true;
        cancelMatchButton.interactable = false;        
    }

    private void OnMatchFound(GetMatchResult getMatchResult)
    {
        ResetButtons();
        gameObject.SetActive(false);
        waitingForPlayers.SetActive(true);

        Authentification.instance.userInfo.currentTeam = int.Parse(getMatchResult.Members.Find(member => member.Entity.Id == Authentification.instance.userInfo.entityId).TeamId);
        GameServer.instance.Connect(
            "127.0.0.1", //getMatchResult.ServerDetails.IPV4Address,
            56100,       //getMatchResult.ServerDetails.Ports[0].Num,
            () => { GameServer.instance.WaitForOtherPlayers(2, OnAllPlayersConnected); } // TODO : Changer le nombre de joueur selon la queue /!\
        );
    }

    private void OnAllPlayersConnected(List<PlayerJoin> players)
    {
        waitingForPlayers.SetActive(false);

        Debug.Log("ENTERING CHAMP SELECT !");
        foreach (PlayerJoin player in players)
        {
            Debug.Log("PLAYER");
            Debug.Log(player.id);
            Debug.Log(player.team);
            Debug.Log(player.isClient);
        }

        //playerSelection.gameObject.SetActive(true);
        //foreach (PlayerJoin player in players)
        //    playerSelection.AddPlayer(player);
    }

    private void OnMatchmakingError(PlayFabError error)
    {
        errorText.text = error.ErrorMessage;
        ResetButtons();
    }
}
