using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.SpawnSystem;
using UnityEngine;
using UnityEngine.UI;
using DrawSystem;

public class BattleManager : MonoBehaviour
{
    public struct BattleTurn {
        public int turnNb;
        public float turnTime;
        public List<Entity> playingEntities;
    }
    public float normalTurnTime;
    public float overtimeTurnTime;
    bool isGameOver = false;

    public Entity currentPlayingEntity;
    BattleTurn battleTurn;
    [Header("Players")]
    public int expectedPlayerNb;
    private bool spawningPhase = true;
    List<Player> battlePlayers;
    public Spawning spawner;

    public GameObject deckButton;
    public GameObject discardButton;
    public Text entityNameText;

    [Header("Spawn Phase")]
    public GameObject spawnDisplay;
    public Text playerNameText;

    [Header("UI Cards")]
    public Deck deck;
    public GameObject discardPile;
    public GameObject hand;

    [Header("UI Turn")]
    public Text timeText;
    public Button finishTurnButton;
    public InitiativeDisplay initiativeDisplay;

    [Header("UI Stats")]
    public StatsSlider statsSlidersDisplay;
    /*
    public Slider healthSlider;
    public Text healthText;
    public Slider shieldSlider;

    public Slider actionSlider;
    public Text actionText;

    public Slider movementSlider;
    public Text movementText;
    */

    [Header("EndGameDisplay")]
    public EndDisplay endDisplay;

    private Player newPlayer;
    
    void Start()
    {
        expectedPlayerNb = GameObject.FindObjectsOfType<Player>().Count();
        battlePlayers = new List<Player>();
    }

    void Update()
    {
        if (!spawningPhase && !isGameOver) {
            battleTurn.turnTime -= Time.deltaTime;
            if (battleTurn.turnTime <= 0) {
                FinishTurn();
            }

            DisplayStats();
        } else {
            playerNameText.text = $"It's {spawner.GetCurrentPlayersName()}'s turn to choose a spawn";
            newPlayer = spawner.SpawningPhase();
            if (newPlayer != null) {
                Debug.Log("add player");
                battlePlayers.Add(newPlayer);
            }

            if (expectedPlayerNb == battlePlayers.Count) {
                spawningPhase = false;
                spawnDisplay.SetActive(false);
                StartGame();
            }
        }
    }

    void StartGame()
    {
        Debug.Log("start game");
        timeText.transform.parent.gameObject.SetActive(true);
        initiativeDisplay.gameObject.SetActive(true);
        statsSlidersDisplay.gameObject.SetActive(true);
        deckButton.SetActive(true);
        discardButton.SetActive(true);

        //Set les valeurs importantes (Deck, DiscardPile, Hand)
        foreach (Player player in battlePlayers) {
            Debug.Log($"{player.playerName} setup");
            player.selectedCharacter.battleManager = this;
            player.deck = deck;
            player.discardPile = discardPile;
            player.hand = hand;
        }

        InitTurn();
        StartTurn();
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
        
        if (battlePlayers.Count == 0) {
            Debug.Log("Warning: No entities with the tag Player found");
            return ;
        }
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
        Debug.Log($"Entity {currentPlayingEntity.entityName}");
        initiativeDisplay.DisplayEntitiesInitiatives(battleTurn.playingEntities);
    }

    public void StartTurn()
    {
        //TODO: si première entité du tour, faut afficher le numéro du tour avant le reste

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
            statsSlidersDisplay.gameObject.SetActive(true);
            //statsSlidersDisplay.healthSlider.gameObject.SetActive(true);
            //statsSlidersDisplay.healthText.gameObject.SetActive(true);
            //statsSlidersDisplay.shieldSlider.gameObject.SetActive(true);
            //statsSlidersDisplay.actionSlider.gameObject.SetActive(true);
            //statsSlidersDisplay.actionText.gameObject.SetActive(true);
            //statsSlidersDisplay.movementSlider.gameObject.SetActive(true);
            //statsSlidersDisplay.movementText.gameObject.SetActive(true);
            //statsSlidersDisplay.healthSlider.maxValue = currentPlayer.selectedCharacter.maxLife;
            //statsSlidersDisplay.healthSlider.value = currentPlayer.selectedCharacter.currentLife;
            //statsSlidersDisplay.healthText.text = $"{currentPlayer.selectedCharacter.currentLife}/{currentPlayer.selectedCharacter.maxLife}";
            //statsSlidersDisplay.shieldSlider.maxValue = currentPlayer.selectedCharacter.maxShield;
            //statsSlidersDisplay.shieldSlider.value = currentPlayer.selectedCharacter.currentShield;
            //statsSlidersDisplay.actionSlider.maxValue = currentPlayer.selectedCharacter.maxActionPoints;
            //statsSlidersDisplay.actionSlider.value = currentPlayer.selectedCharacter.currentActionPoints;
            //statsSlidersDisplay.actionText.text = $"{currentPlayer.selectedCharacter.currentActionPoints}/{currentPlayer.selectedCharacter.maxActionPoints}";
            //statsSlidersDisplay.movementSlider.maxValue = currentPlayer.selectedCharacter.maxMovePoints;
            //statsSlidersDisplay.movementSlider.value = currentPlayer.selectedCharacter.currentMovePoints;
            //statsSlidersDisplay.movementText.text = $"{currentPlayer.selectedCharacter.currentMovePoints}/{currentPlayer.selectedCharacter.maxMovePoints}";
            statsSlidersDisplay.SetInfos(currentPlayer.selectedCharacter, false);
        } else {
            statsSlidersDisplay.gameObject.SetActive(false);
            //healthSlider.gameObject.SetActive(false);
            //healthText.gameObject.SetActive(false);
            //shieldSlider.gameObject.SetActive(false);
            //actionSlider.gameObject.SetActive(false);
            //actionText.gameObject.SetActive(false);
            //movementSlider.gameObject.SetActive(false);
            //movementText.gameObject.SetActive(false);
        }

        if (currentPlayer.deck.isDrawingOver) {
            finishTurnButton.gameObject.SetActive(true);
        } else {
            finishTurnButton.gameObject.SetActive(false);
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

        if (battleTurn.turnNb < 10) {
            battleTurn.turnTime = normalTurnTime;
        } else {
            battleTurn.turnTime = overtimeTurnTime;
        }
        battleTurn.playingEntities.RemoveAt(0);
        if (battleTurn.playingEntities.Count == 0) {
            InitTurn();
        }
        currentPlayingEntity = battleTurn.playingEntities[0];
        StartTurn();
    }

    public void RemovePlayer(Player player)
    {
        if (player.selectedCharacter == currentPlayingEntity) {
            //TODO: quand ca sera multijoueur, ajouter un menu qui permet de continuer à spectate, ou de retourner au menu
            endDisplay.gameObject.SetActive(true);
            endDisplay.DisplayDefeat(player);
            player.EndTurn();
            currentPlayingEntity.EndTurn();
        }

        initiativeDisplay.RemoveFromTimeline(player.selectedCharacter);
        battlePlayers.Remove(player);

        battleTurn.playingEntities.Remove(player.selectedCharacter);
        foreach (Entity entity in player.selectedCharacter.alliedEntities) {
            battleTurn.playingEntities.Remove(entity);
        }

        if (battlePlayers.Count == 1) {
            isGameOver = true;
            battlePlayers[0].selectedCharacter.canMove = false;
            //TODO: empecher au joueur de poser des cartes
            //TODO: passer la souris sur la map ne doit plus rien faire
            endDisplay.gameObject.SetActive(true);
            endDisplay.DisplayVictory(battlePlayers[0]);

            return ;
        } else if (battlePlayers.Count == 0) {
            endDisplay.gameObject.SetActive(true);
            endDisplay.DisplayDraw();
        }
    }

    public void RemoveEntity(Entity entity)
    {
        initiativeDisplay.RemoveFromTimeline(entity);
        battleTurn.playingEntities.Remove(entity);
    }
}
