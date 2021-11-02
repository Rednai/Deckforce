using UnityEngine;
using UnityEngine.UI;

namespace DrawSystem
{
    public class DiscardText : MonoBehaviour
    {
        public GameObject discardPile;
        void Update()
        {
            GetComponent<Text>().text = "Discard Pile: " + discardPile.transform.childCount;
        }
    }
}