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
        endText.text = $"{player.playerName} won!";
    }

    public void DisplayDefeat(Player player)
    {
        endText.text = $"{player.playerName} lost!";

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

        for (int i = players.Length-1; i != -1; i--) {
            Destroy(players[i].gameObject);
        }
        SceneManager.LoadScene("MainMenuMultiplayer");
    }
}
