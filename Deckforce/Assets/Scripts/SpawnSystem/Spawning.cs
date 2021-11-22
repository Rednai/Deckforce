using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.SpawnSystem
{
    public class Spawning : MonoBehaviour
    {
        [SerializeField]public GameObject playerPrefab;
        private Tile currentSelected = null;
        private Player newPlayer;
        public List<Player> futurePlayers;

        public Player SpawningPhase()
        {
            currentSelected = this.GetComponent<SelectCase>().currentSelected;

            if (Input.GetMouseButtonDown(1) && currentSelected != null && futurePlayers.Count > 0 && currentSelected.tileEntity == null) {
                newPlayer = Instantiate(futurePlayers[0].gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
                futurePlayers.RemoveAt(0);
                newPlayer.selectedCharacter = newPlayer.transform.GetChild(0).GetComponent<Character>();
                newPlayer.selectedCharacter.transform.position = new Vector3(currentSelected.transform.position.x, 0.5f, currentSelected.transform.position.z);
                newPlayer.selectedCharacter.GetComponent<Pathfinding>().setStartTile(currentSelected);
                newPlayer.gameObject.SetActive(true);
                currentSelected.tileEntity = newPlayer.selectedCharacter;
                return newPlayer;
            }

            return null;
        }
    }
}