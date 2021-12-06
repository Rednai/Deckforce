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

        Authentification.instance.userInfo.currentTeam =
            int.Parse(getMatchResult.Members.Find(member => member.Entity.Id == Authentification.instance.userInfo.entityId).TeamId);
        GameServer.instance.Connect(
            getMatchResult.ServerDetails.IPV4Address,
            getMatchResult.ServerDetails.Ports[0].Num, () => {
                GameServer.instance.WaitForOtherPlayers(2, OnAllPlayersConnected); // TODO : Changer le nombre de joueur selon la queue /!\
            });
    }

    private void OnAllPlayersConnected(List<PlayerJoin> players)
    {
        waitingForPlayers.SetActive(false);

    }

    private void OnMatchmakingError(PlayFabError error)
    {
        errorText.text = error.ErrorMessage;
        ResetButtons();
    }
}
