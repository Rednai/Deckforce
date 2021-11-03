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
    public List<Entity> playerAlliedEntitites;
    
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

    void Update()
    {
        DisplayStats();
    }

    public void StartTurn()
    {
        currentPlayer.StartTurn();
        currentPlayer.selectedCharacter.StartTurn();
        playerAlliedEntitites = new List<Entity>(currentPlayer.selectedCharacter.alliedEntities);
        
        playerNameText.text = $"{currentPlayer.playerName}'s turn";
        
        DisplayStats();
        //TODO: focus la caméra sur le joueur actuel
    }

    void DisplayStats()
    {
        healthSlider.maxValue = currentPlayer.selectedCharacter.maxLife;
        healthSlider.value = currentPlayer.selectedCharacter.currentLife;
        healthText.text = $"{currentPlayer.selectedCharacter.currentLife}/{currentPlayer.selectedCharacter.maxLife}";

        actionSlider.maxValue = currentPlayer.selectedCharacter.maxActionPoints;
        actionSlider.value = currentPlayer.selectedCharacter.currentActionPoints;
        actionText.text = $"{currentPlayer.selectedCharacter.currentActionPoints}/{currentPlayer.selectedCharacter.maxActionPoints}";

        movementSlider.maxValue = currentPlayer.selectedCharacter.maxMovePoints;
        movementSlider.value = currentPlayer.selectedCharacter.currentMovePoints;
        movementText.text = $"{currentPlayer.selectedCharacter.currentMovePoints}/{currentPlayer.selectedCharacter.maxMovePoints}";
    }

    public void FinishTurn()
    {
        currentPlayer.EndTurn();
        currentPlayer.selectedCharacter.canMove = false;
        if (currentPlayer == firstPlayer) {
            currentPlayer = secondPlayer;
            StartTurn();
            return ;
        }
        currentPlayer = firstPlayer;
        StartTurn();
    }
}
