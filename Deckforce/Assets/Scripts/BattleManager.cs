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
        public int playingEntityIndex;
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

    [Header("EndGameDisplay")]
    public EndDisplay endDisplay;

    private Player newPlayer;

    GameServer gameServer;

    //TODO: temporaire
    public string tileName;
    
    void Start()
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        expectedPlayerNb = players.Count();
        battlePlayers = new List<Player>();
        gameServer = GameServer.FindObjectOfType<GameServer>();
        gameServer.GetComponent<Parser>().InitValues(players);
    }

    void Update()
    {
        //TODO: faire un if global avec isGameOver et ensuite check spwaningPhase
        if (!isGameOver) {
            if (!spawningPhase) {
                if (battleTurn.turnTime <= 0) {
                    if (gameServer == null) {
                        gameServer = GameServer.FindObjectOfType<GameServer>();
                    }

                    SkipTurn skipTurn = new SkipTurn();
                    skipTurn.entityPlayingIndex = battleTurn.playingEntityIndex;
                    gameServer.SendData(skipTurn);
                    
                    FinishTurn();
                }

                DisplayStats();
            } else {
                if (spawner.GetCurrentPlayer().isClient) {
                    playerNameText.text = $"It's your turn to choose a spawn";
                } else {
                    playerNameText.text = $"It's {spawner.GetCurrentPlayer().username}'s turn to choose a spawn";
                }
                newPlayer = spawner.SpawningPhase();

                if (newPlayer != null) {
                    PlayerSpawn playerSpawn = new PlayerSpawn();
                    playerSpawn.playerId = newPlayer.id;
                    playerSpawn.tileName = tileName;
                    gameServer.SendData(playerSpawn);
                    AddPlayer(newPlayer);
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (!isGameOver && !spawningPhase) {
            battleTurn.turnTime -= Time.deltaTime;
        }
    }

    public void AddPlayer(Player newPlayer)
    {
        battlePlayers.Add(newPlayer);
        if (expectedPlayerNb == battlePlayers.Count)
        {
            spawner.StopHighlightingSpawnTiles();
            spawningPhase = false;
            spawnDisplay.SetActive(false);
            StartGame();
        }
    }

    void StartGame()
    {
        timeText.transform.parent.gameObject.SetActive(true);
        initiativeDisplay.gameObject.SetActive(true);
        statsSlidersDisplay.gameObject.SetActive(true);
        deckButton.SetActive(true);
        discardButton.SetActive(true);

        foreach (Player player in battlePlayers) {
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
        battleTurn.playingEntityIndex = 0;
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
        battleTurn.playingEntityIndex++;
        initiativeDisplay.DisplayEntitiesInitiatives(battleTurn.playingEntities);
    }

    public int GetPlayingEntityIndex()
    {
        return (battleTurn.playingEntityIndex);
    }

    public void StartTurn()
    {
        //TODO: si première entité du tour, faut afficher le numéro du tour avant le reste

        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter && player.isClient) {
                player.StartTurn();
                statsSlidersDisplay.SetInfos(player.selectedCharacter, false);
            }
        }
        //TODO: faire en sorte que pour les IA ca finisse le Tour automatiquement
        currentPlayingEntity.StartTurn();

        //if (currentPlayingEntity == player.selectedCharacter && player.isClient) {

        Player currentPlayer = null;
        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter && player.isClient) {
                currentPlayer = player;
            }
        }
        
        if (currentPlayer != null) {
            entityNameText.text = "Your turn";
        } else {
            entityNameText.text = $"{currentPlayingEntity.entityName}'s turn";
        }

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
        
            if (currentPlayer.deck.isDrawingOver) {
                finishTurnButton.gameObject.SetActive(true);
            } else {
                finishTurnButton.gameObject.SetActive(false);
            }
        } else {
            statsSlidersDisplay.gameObject.SetActive(false);
        }
    }

    Player CheckIfCurrentEntityIsPlayer()
    {
        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter && player.isClient) {
                return (player);
            }
        }
        return (null);
    }

    public void ClickFinishTurn()
    {
        if (gameServer == null) {
            gameServer = GameServer.FindObjectOfType<GameServer>();
        }

        SkipTurn skipTurn = new SkipTurn();
        skipTurn.entityPlayingIndex = battleTurn.playingEntityIndex;
        gameServer.SendData(skipTurn);
        
        FinishTurn();
    }

    public void FinishTurn()
    {
        if (finishTurnButton.gameObject.activeInHierarchy)
            finishTurnButton.gameObject.SetActive(false);
        foreach (Player player in battlePlayers) {
            if (currentPlayingEntity == player.selectedCharacter) {
                player.EndTurn();
                statsSlidersDisplay.ResetEffects();
            }
        }
        //TODO: faire en sorte que pour les IA ca finisse le Tour automatiquement
        
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
        battleTurn.playingEntityIndex++;
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
