using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Players")]
    public Player firstPlayer;
    public Player secondPlayer;

    public Player currentPlayer;
    
    [Header("UI")]
    public Text playerNameText;
    public Button finishTurnButton;

    public Slider healthSlider;
    public Text healthText;

    public Slider actionSlider;
    public Text actionText;

    public Slider movementSlider;
    public Text movementText;

    void Start()
    {
        currentPlayer = firstPlayer;
        StartTurn();
    }

    public void StartTurn()
    {
        currentPlayer.selectedCharacter.StartTurn();
        
        playerNameText.text = $"{currentPlayer.playerName}'s turn";
        
        healthSlider.maxValue = currentPlayer.selectedCharacter.maxLife;
        healthSlider.value = currentPlayer.selectedCharacter.maxLife;
        healthText.text = $"{currentPlayer.selectedCharacter.maxLife}/{currentPlayer.selectedCharacter.maxLife}";

        actionSlider.maxValue = currentPlayer.selectedCharacter.maxActionPoints;
        actionSlider.value = currentPlayer.selectedCharacter.maxActionPoints;
        actionText.text = $"{currentPlayer.selectedCharacter.maxActionPoints}/{currentPlayer.selectedCharacter.maxActionPoints}";

        movementSlider.maxValue = currentPlayer.selectedCharacter.maxMovePoints;
        movementSlider.value = currentPlayer.selectedCharacter.maxMovePoints;
        movementText.text = $"{currentPlayer.selectedCharacter.maxMovePoints}/{currentPlayer.selectedCharacter.maxMovePoints}";

        //TODO: focus la caméra sur le joueur actuel
    }

    public void FinishTurn()
    {
        if (currentPlayer == firstPlayer) {
            currentPlayer = secondPlayer;
            StartTurn();
            return ;
        }
        currentPlayer = firstPlayer;
        StartTurn();
    }
}
