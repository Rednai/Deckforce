using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.SpawnSystem
{
    public class Spawning : MonoBehaviour
    {
        [SerializeField]public GameObject playerPrefab;
        private Tile currentSelected = null;
        private Player newPlayer;

        public void SpawnPlayer()
        {
            currentSelected = this.GetComponent<SelectCase>().currentSelected;

            if (Input.GetMouseButtonDown(1) && currentSelected != null) {
                newPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
                newPlayer.playerName = "coco";
                newPlayer.selectedCharacter = newPlayer.transform.GetChild(0).GetComponent<Character>();
                newPlayer.selectedCharacter.transform.position = new Vector3(currentSelected.transform.position.x, 0.5f, currentSelected.transform.position.z);
            }
        }
    }
}
