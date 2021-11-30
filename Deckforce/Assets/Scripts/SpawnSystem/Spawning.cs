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

            if (Input.GetMouseButtonDown(1) && currentSelected != null && futurePlayers.Count > 0 && currentSelected.tileEntity == null) {
                //newPlayer = Instantiate(futurePlayers[0].gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
                newPlayer = futurePlayers[0];
                futurePlayers.RemoveAt(0);
                //newPlayer.selectedCharacter = newPlayer.transform.GetChild(0).GetComponent<Character>();

                newPlayer.selectedCharacter.transform.position = new Vector3(currentSelected.transform.position.x, 0.5f, currentSelected.transform.position.z);
                newPlayer.selectedCharacter.GetComponent<Pathfinding>().setStartTile(currentSelected);
                newPlayer.selectedCharacter.gameObject.SetActive(true);
                currentSelected.tileEntity = newPlayer.selectedCharacter;
                if (futurePlayers.Count == 0)
                    this.GetComponent<SelectCase>().spawningMode = false;
                return newPlayer;
            }

            return null;
        }

        public string GetCurrentPlayersName()
        {
            return (futurePlayers[0].playerName);
        }
    }
}
