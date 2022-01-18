using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class EndDisplay : MonoBehaviour
{
    public Button spectateButton;
    public Text endText;

    public void DisplayVictory(Player player)
    {
        if (player.isClient && !GameServer.instance.isOffline) {
            endText.text = "You won!";
        } else {
            endText.text = $"{player.username} won!";
        }

        spectateButton.gameObject.SetActive(false);
    }

    public void DisplayDefeat(Player player)
    {
        if (player.isClient) {
            endText.text = "You lost!";
        } else {
            endText.text = $"{player.username} lost!";
        }

        spectateButton.gameObject.SetActive(true);
    }

    public void DisplayDraw()
    {
        endText.text = "Draw!";
    }

    public void Spectate()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        bool clientIsTeamOne = false;

        for (int i = players.Length-1; i != -1; i--) {
            if (players[i].isClient && players[i].team == 1)
                clientIsTeamOne = true;
            Destroy(players[i].gameObject);
        }

        if (clientIsTeamOne)
            GameServer.instance.StopServer();
        else
            GameServer.instance.Disconnect();

        SceneManager.LoadScene("MainMenuMultiplayer");
    }
}
