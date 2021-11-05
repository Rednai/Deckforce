using UnityEngine;
using UnityEngine.UI;

namespace DrawSystem
{
    public class DeckText : MonoBehaviour
    {
        private int count = 0;
        public GameObject deck;
        void Update()
        {
            foreach(Transform child in deck.transform){
                if(child.gameObject.activeSelf)
                    count++;
            }
            GetComponent<Text>().text = "Deck: " + count;
            count = 0;
        }
    }
}
