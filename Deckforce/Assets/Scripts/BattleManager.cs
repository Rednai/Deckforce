using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public struct BattleTurn {
        public int turnNb;
        public float turnTime;
        public List<Entity> playingEntities;
    }
    public float normalTurnTime;
    public float overtimeTurnTime;

    Entity currentPlayingEntity;
    BattleTurn battleTurn;
    [Header("Players")]
    public List<Player> battlePlayers;

    public Text entityNameText;
    [Header("UI Turn")]
    public Text timeText;
    public Button finishTurnButton;
    public InitiativeDisplay initiativeDisplay;

    [Header("UI Stats")]
    public Slider healthSlider;
    public Text healthText;

    public Slider actionSlider;
    public Text actionText;

    public Slider movementSlider;
    public Text movementText;

    void Start()
    {
        //currentPlayer = firstPlayer;
        //currentPlayingEntity = battleTurn.playingEntities[0];
        InitTurn();
        StartTurn();
    }

    void Update()
    {
        battleTurn.turnTime -= Time.deltaTime;
        if (battleTurn.turnTime <= 0) {
            FinishTurn();
        }

        DisplayStats();
    }

    void InitTurn()
    {
        battleTurn = new BattleTurn();

        battleTurn.turnNb = battleTurn.turnNb + 1;
        if (battleTurn.turnNb < 10) {
            battleTurn.turnTime = normalTurnTime;
        } else {
            battleTurn.turnTime = overtimeTurnTime;
        }

        battleTurn.playingEntities = new List<Entity>();
        foreach (Player player in battlePlayers) {
            battleTurn.playingEntities.Add(player.selectedCharacter);
            foreach (Entity alliedEntity in player.selectedCharacter.alliedEntities) {
                battleTurn.playingEntities.Add(alliedEntity);
            }
        }
        battleTurn.playingEntities.Sort(delegate(Entity E1, Entity E2) {
            return (-E1.initiative.CompareTo(E2.initiative));
        });
        currentPlayingEntity = battleTurn.playingEntities[0];
        initiativeDisplay.DisplayEntitiesInitiatives(battleTurn.playingEntities);
    }

    public void StartTurn()
    {
        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter) {
                player.StartTurn();
            }
        }
        //TODO: faire en sorte que pour les IA ca finisse le Tour automatiquement
        currentPlayingEntity.StartTurn();
        initiativeDisplay.AdvanceInitiative(currentPlayingEntity);
        
        entityNameText.text = $"{currentPlayingEntity.entityName}'s turn";
        
        DisplayStats();
        //TODO: focus la caméra sur le joueur actuel
    }

    void DisplayStats()
    {
        int seconds = (int)(battleTurn.turnTime % 60);
        timeText.text = "Time remaining: " + string.Format("{0:00}", seconds);

        Player currentPlayer = CheckIfCurrentEntityIsPlayer();
        if (currentPlayer != null) {
            healthSlider.gameObject.SetActive(true);
            healthText.gameObject.SetActive(true);
            actionSlider.gameObject.SetActive(true);
            actionText.gameObject.SetActive(true);
            movementSlider.gameObject.SetActive(true);
            movementText.gameObject.SetActive(true);
            healthSlider.maxValue = currentPlayer.selectedCharacter.maxLife;
            healthSlider.value = currentPlayer.selectedCharacter.currentLife;
            healthText.text = $"{currentPlayer.selectedCharacter.currentLife}/{currentPlayer.selectedCharacter.maxLife}";

            actionSlider.maxValue = currentPlayer.selectedCharacter.maxActionPoints;
            actionSlider.value = currentPlayer.selectedCharacter.currentActionPoints;
            actionText.text = $"{currentPlayer.selectedCharacter.currentActionPoints}/{currentPlayer.selectedCharacter.maxActionPoints}";

            movementSlider.maxValue = currentPlayer.selectedCharacter.maxMovePoints;
            movementSlider.value = currentPlayer.selectedCharacter.currentMovePoints;
            movementText.text = $"{currentPlayer.selectedCharacter.currentMovePoints}/{currentPlayer.selectedCharacter.maxMovePoints}";
        } else {
            healthSlider.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            actionSlider.gameObject.SetActive(false);
            actionText.gameObject.SetActive(false);
            movementSlider.gameObject.SetActive(false);
            movementText.gameObject.SetActive(false);
        }
    }

    Player CheckIfCurrentEntityIsPlayer()
    {
        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter) {
                return (player);
            }
        }
        return (null);
    }

    public void FinishTurn()
    {
        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter) {
                player.EndTurn();
            }
        }
        //TODO: faire en sorte que pour les IA ca finisse le Tour automatiquement
        currentPlayingEntity.EndTurn();
        currentPlayingEntity.canMove = false;
        initiativeDisplay.RemoveFromTimeline(currentPlayingEntity);

        battleTurn.playingEntities.RemoveAt(0);
        if (battleTurn.playingEntities.Count == 0) {
            InitTurn();
        }
        currentPlayingEntity = battleTurn.playingEntities[0];
        StartTurn();
    }
}
