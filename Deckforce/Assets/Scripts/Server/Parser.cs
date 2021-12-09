using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SpawnSystem;

public class Parser : MonoBehaviour
{
    public static Parser instance;

    public BattleManager battleManager;
    public Spawning spawning;
    //TODO: récupérer les joueurs dans la classe du serveur
    public List<Player> players;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void InitValues(Player[] newPlayers)
    {
        players = new List<Player>(newPlayers);
        battleManager = GameObject.FindObjectOfType<BattleManager>();
        spawning = GameObject.FindObjectOfType<Spawning>();
    }

    public void ParseData(object obj)
    {
        switch (obj) {
            case ActivateCard cardObj:
                //récupère la carte depuis le CardManager avec l'id et l'active à la position choisie
                CardsManager.instance.cards.Find(x => x.id == cardObj.cardId).Activate(
                    players.Find(x => x.id == cardObj.playerId),
                    GameObject.Find(cardObj.tileName).GetComponent<Tile>()
                );
                break;
            case PlayerMove moveObj:
                //TODO: déplace le joueur a une position choisie
                Debug.Log("on bouge l'autre joueur incroyable");
                Character character = players.Find(x => x.id == moveObj.playerId).selectedCharacter;
                MovePlayer movePlayer = character.GetComponent<MovePlayer>();
                Tile selectedTile = GameObject.Find(moveObj.tileName).GetComponent<Tile>();

                Debug.Log(character.entityName);
                movePlayer.MoveCharacter(selectedTile, movePlayer.pathfinding.findPathtoCase(selectedTile));

                break;
            case PlayerSpawn spawnObj:
                battleManager.AddPlayer(spawning.SpawnOtherPlayer(spawnObj.playerId, spawnObj.tileName));
                break;
            case SkipTurn turnObj:
                //passe au tour suivant
                if (battleManager.GetPlayingEntityIndex() == turnObj.entityPlayingIndex) {
                    battleManager.FinishTurn();
                }

                //TODO: système de vérification si jamais le joueur a du retard sur le tour actuel
                //Si par exemple un packet skipturn est recu juste après que le tour soit fini, il y aura peut etre un petit décalage
                //Peut etre entre chaque tour, mettre un petit délai de 5 secondes pour vérifier que tous les clients sont bien à jour
                break;
            case ChooseCharacter characterObj:
                Debug.Log("other player changed his character: " + characterObj.characterId);
                GameObject.FindObjectOfType<PlayersSelection>().SetPlayerCharacter(characterObj);
                break;
            case PlayerJoin playerJoin:
                GameServer.instance.OnPlayerJoin(playerJoin);
                break;
            case PlayerReady readyObj:
                GameObject.FindObjectOfType<PlayersSelection>().SetPlayerReady(readyObj);
                break;
        }
    }
}
