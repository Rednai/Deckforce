using UnityEngine;
using UnityEngine.UI;

namespace DrawSystem
{
    public class DeckText : MonoBehaviour
    {
        public GameObject deck;
        void Update()
        {
            GetComponent<Text>().text = "Deck: " + deck.transform.childCount;
        }
    }
}
