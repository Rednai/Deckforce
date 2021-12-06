using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SpawnSystem;

public class Parser : MonoBehaviour
{
    public static Parser instance;

    public CardsManager cardsManager;
    public CharactersManager charactersManager;
    public BattleManager battleManager;
    public Spawning spawning;
    //TODO: récupérer les joueurs dans la classe du serveur
    public List<Player> players;

    private void Awake()
    {
        instance = this;
    }

    public void ParseData(object obj)
    {
        switch (obj) {
            case ActivateCard cardObj:
                //récupère la carte depuis le CardManager avec l'id et l'active à la position choisie
                cardsManager.cards.Find(x => x.id == cardObj.cardId).Activate(
                    players.Find(x => x.id == cardObj.playerId),
                    GameObject.Find(cardObj.tileName).GetComponent<Tile>()
                );
                break;
            case PlayerMove moveObj:
                //TODO: déplace le joueur a une position choisie
                Character character = players.Find(x => x.id == moveObj.playerId).selectedCharacter;
                MovePlayer movePlayer = character.GetComponent<MovePlayer>();
                Tile selectedTile = GameObject.Find(moveObj.tileName).GetComponent<Tile>();

                movePlayer.MoveCharacter(selectedTile, movePlayer.pathfinding.findPathtoCase(selectedTile));

                break;
            case PlayerSpawn spawnObj:
                if (battleManager == null) {
                    battleManager = GameObject.FindObjectOfType<BattleManager>();
                }
                //fais spawn le joueur a une position
                if (spawning == null) {
                    spawning = GameObject.FindObjectOfType<Spawning>();
                }
                battleManager.AddPlayer(spawning.SpawnOtherPlayer(spawnObj.playerId, spawnObj.tileName));
                break;
            case SkipTurn turnObj:
                //passe au tour suivant
                if (battleManager == null) {
                    battleManager = GameObject.FindObjectOfType<BattleManager>();
                }
                if (turnObj.entityPlayingIndex == battleManager.GetPlayingEntityIndex()) {
                    battleManager.FinishTurn();
                }
                //TODO: système de vérification si jamais le joueur a du retard sur le tour actuel
                break;
            case ChooseCharacter characterObj:
                GameObject.FindObjectOfType<PlayersSelection>().SetPlayerCharacter(characterObj);
                break;
            case PlayerJoin playerJoin:
                GameServer.instance.OnPlayerJoin(playerJoin);
                break;
        }
    }
}
