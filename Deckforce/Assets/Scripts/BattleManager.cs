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
    public int battleId;
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

    //TODO: temporaire
    public string tileName;
    
    void Start()
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        expectedPlayerNb = players.Count();
        battlePlayers = new List<Player>();
        GameServer.instance.GetComponent<Parser>().InitValues(players);

        battleId = 0;
    }

    void Update()
    {
        //TODO: faire un if global avec isGameOver et ensuite check spwaningPhase
        if (!isGameOver) {
            if (!spawningPhase) {
                if (battleTurn.turnTime <= 0) {
                    
                    if (!GameServer.instance.isOffline) {
                        SkipTurn skipTurn = new SkipTurn();
                        skipTurn.entityPlayingIndex = battleTurn.playingEntityIndex;
                        GameServer.instance.SendData(skipTurn);
                    }
                    
                    FinishTurn();
                }

                DisplayStats();
            } else {
                if (spawner.GetCurrentPlayer().isClient && !GameServer.instance.isOffline) {
                    playerNameText.text = $"It's your turn to choose a spawn";
                } else {
                    playerNameText.text = $"It's {spawner.GetCurrentPlayer().username}'s turn to choose a spawn";
                }
                newPlayer = spawner.SpawningPhase();

                if (newPlayer != null) {
                    if (!GameServer.instance.isOffline) {
                        PlayerSpawn playerSpawn = new PlayerSpawn();
                        playerSpawn.playerId = newPlayer.id;
                        playerSpawn.tileName = tileName;
                        GameServer.instance.SendData(playerSpawn);
                    }
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
            player.selectedCharacter.battleId = battleId;
            battleId++;
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
            if (E1.initiative == E2.initiative) {
                return (E1.battleId.CompareTo(E2.battleId));
            } else {
                return (-E1.initiative.CompareTo(E2.initiative));
            }
        });
        currentPlayingEntity = battleTurn.playingEntities[0];
        battleTurn.playingEntityIndex++;
        initiativeDisplay.ResetDisplay();
        initiativeDisplay.DisplayEntitiesInitiatives(battleTurn.playingEntities);
    }

    public int GetPlayingEntityIndex()
    {
        return (battleTurn.playingEntityIndex);
    }

    public void StartTurn()
    {
        //TODO: si première entité du tour, faut afficher le numéro du tour avant le reste

        Player currentPlayer = null;
        foreach (Player battlePlayer in battlePlayers) {
            if (battlePlayer.id == currentPlayingEntity.playerId) {
                currentPlayer = battlePlayer;
            }
        }

        if (GameServer.instance.isOffline) {
            if (currentPlayer.selectedCharacter == currentPlayingEntity) {
                entityNameText.text = $"{currentPlayer.username}'s turn";
                currentPlayer.StartTurn();
                statsSlidersDisplay.SetInfos(currentPlayer.selectedCharacter, false);
            } else {
                entityNameText.text = $"{currentPlayer.username}'s ally turn";
            }
        } else {
            if (currentPlayer.isClient) {
                //TODO: a voir si faut pas utiliser le battleId plutot
                if (currentPlayer.selectedCharacter == currentPlayingEntity) {
                    entityNameText.text = "Your turn";
                    currentPlayer.StartTurn();
                    statsSlidersDisplay.SetInfos(currentPlayer.selectedCharacter, false);
                } else {
                    entityNameText.text = "Your ally's turn";
                }
            } else {
                if (currentPlayer.selectedCharacter == currentPlayingEntity) {
                    entityNameText.text = $"{currentPlayer.username}'s turn";
                } else {
                    entityNameText.text = $"{currentPlayer.username}'s ally turn";
                }
            }
        }
        currentPlayingEntity.StartTurn();

        /*
        Player player = CheckIfCurrentEntityIsPlayer();
        if (player != null) {
            player.StartTurn();
            statsSlidersDisplay.SetInfos(player.selectedCharacter, false);
            entityNameText.text = "Your turn";
        } else {
            //1 pas le charactère de notre joueur mais un de ses alliés

            //2 le charactère d'un autre joueur
            //3 un des alliés du charactère d'un autre joueur
        }
        currentPlayingEntity.StartTurn();

        if (currentPlayer != null) {
            
        } else {
            //TODO: ca marchera pas, player sera vide
            if (player.selectedCharacter.alliedEntities.Contains(currentPlayingEntity)) {
                entityNameText.text = $"{player.username}'s monster turn";
            } else {
                Player otherPlayer = null;
                foreach (Player p in battlePlayers) {
                    if () {
                        otherPlayer = p;
                    }
                }
                if () {

                } else {

                }
                entityNameText.text = $"{currentPlayingEntity.entityName}'s turn";
            }
        }
        */

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
            if (currentPlayingEntity == player.selectedCharacter && player.isClient && player.id == currentPlayingEntity.playerId) {
                return (player);
            }
        }
        return (null);
    }

    public void ClickFinishTurn()
    {
        if (!GameServer.instance.isOffline) {
            SkipTurn skipTurn = new SkipTurn();
            skipTurn.entityPlayingIndex = battleTurn.playingEntityIndex;
            GameServer.instance.SendData(skipTurn);
        }
        
        FinishTurn();
    }

    public void FinishOtherPlayerTurn()
    {
        if (battlePlayers == null || isGameOver) {
            return ;
        }
        foreach (Player player in battlePlayers) {
            if (player.selectedCharacter == currentPlayingEntity && currentPlayingEntity.playerId == player.id) {
                player.EndTurn();
                player.selectedCharacter.EndTurn();
            }
        }
        FinishTurn();
    }

    public void FinishTurn()
    {
        if (finishTurnButton.gameObject.activeInHierarchy)
            finishTurnButton.gameObject.SetActive(false);
        Player player = CheckIfCurrentEntityIsPlayer();
        if (player != null) {
            player.EndTurn();
            statsSlidersDisplay.ResetEffects();
            player.selectedCharacter.EndTurn();
        }
        
        initiativeDisplay.SetBackInTimeline(currentPlayingEntity);
        //initiativeDisplay.RemoveFromTimeline(currentPlayingEntity);

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
        if (battlePlayers.Count == 2) {
            //TODO: empecher au joueur de poser des cartes
            //TODO: passer la souris sur la map ne doit plus rien faire
            battlePlayers.Remove(player);
            isGameOver = true;
            battlePlayers[0].selectedCharacter.canMove = false;
            endDisplay.gameObject.SetActive(true);
            endDisplay.DisplayVictory(battlePlayers[0]);
        } else if (battlePlayers.Count == 1) {
            battlePlayers.Remove(player);
            isGameOver = true;
            endDisplay.gameObject.SetActive(true);
            endDisplay.DisplayDraw();
        } else {
            if (player.isClient) {
            //if (player.selectedCharacter == currentPlayingEntity) {
                //TODO: quand ca sera multijoueur, ajouter un menu qui permet de continuer à spectate, ou de retourner au menu
                endDisplay.gameObject.SetActive(true);
                endDisplay.DisplayDefeat(player);
                player.EndTurn();
                currentPlayingEntity.EndTurn();
            } else {
                foreach (Player otherPlayer in battlePlayers) {
                    if (otherPlayer.selectedCharacter == currentPlayingEntity && currentPlayingEntity.playerId == otherPlayer.id) {
                        otherPlayer.EndTurn();
                        currentPlayingEntity.EndTurn();
                    }
                }
            }
            initiativeDisplay.RemoveFromTimeline(player.selectedCharacter);

            battleTurn.playingEntities.Remove(player.selectedCharacter);
            foreach (Entity entity in player.selectedCharacter.alliedEntities) {
                battleTurn.playingEntities.Remove(entity);
            }
        }
    }

    public void RemoveEntity(Entity entity)
    {
        entity.EndTurn();
        initiativeDisplay.RemoveFromTimeline(entity);
        battleTurn.playingEntities.Remove(entity);
    }
}
