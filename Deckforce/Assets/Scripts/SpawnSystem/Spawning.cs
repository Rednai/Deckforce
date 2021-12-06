using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.SpawnSystem
{
    public class Spawning : MonoBehaviour
    {
        private Tile currentSelected = null;
        private Player newPlayer;
        List<Player> futurePlayers;

        public List<Player> savePlayers;

        void Start()
        {
            //TODO: récupérer cette liste dans le serveur
            futurePlayers = new List<Player>(GameObject.FindObjectsOfType<Player>());

            Debug.Log(futurePlayers.Count);
            //Appelé si la scène est lancée telle quelle. Plus pratique quand il faut tester qqchose sur la scène de combat
            if (futurePlayers.Count == 0) {
                futurePlayers = new List<Player>();
                foreach (Player player in savePlayers) {
                    player.gameObject.SetActive(true);
                    futurePlayers.Add(player);
                }
                Debug.Log(futurePlayers.Count);
            }
        }

        public Player SpawningPhase()
        {
            currentSelected = this.GetComponent<SelectCase>().currentSelected;

            //TODO: ca doit fonctionner uniquement si c'est notre tour
            if (Input.GetMouseButtonDown(1) && currentSelected != null &&
                futurePlayers.Count > 0 && currentSelected.tileEntity == null
                ) {
                return (SpawnPlayer());
            }

            return null;
        }

        public Player SpawnPlayer()
        {
            newPlayer = futurePlayers[0];
            futurePlayers.RemoveAt(0);

            newPlayer.selectedCharacter.transform.position = new Vector3(currentSelected.transform.position.x, 0.5f, currentSelected.transform.position.z);
            newPlayer.selectedCharacter.GetComponent<Pathfinding>().setStartTile(currentSelected);
            newPlayer.selectedCharacter.gameObject.SetActive(true);
            currentSelected.SetEntity(newPlayer.selectedCharacter);
            if (futurePlayers.Count == 0)
                this.GetComponent<SelectCase>().spawningMode = false;
            return newPlayer;
        }

        public Player SpawnOtherPlayer(string playerId, string tileName)
        {
            newPlayer = futurePlayers.Find(x => x.id == playerId);
            futurePlayers.Remove(newPlayer);

            Tile selectedTile = GameObject.Find(tileName).GetComponent<Tile>();
            newPlayer.selectedCharacter.transform.position = new Vector3(selectedTile.transform.position.x, 0.5f, selectedTile.transform.position.z);
            newPlayer.selectedCharacter.GetComponent<Pathfinding>().setStartTile(selectedTile);
            newPlayer.selectedCharacter.gameObject.SetActive(true);
            selectedTile.SetEntity(newPlayer.selectedCharacter);
            if (futurePlayers.Count == 0)
                this.GetComponent<SelectCase>().spawningMode = false;
            return (newPlayer);
        }

        public string GetCurrentPlayersName()
        {
            return (futurePlayers[0].playerName);
        }
    }
}
