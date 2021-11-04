using UnityEngine;
using UnityEngine.UI;

namespace DrawSystem
{
    public class DiscardText : MonoBehaviour
    {
        private int count = 0;
        public GameObject discardPile;
        void Update()
        {
            foreach(Transform child in discardPile.transform){
                if(child.gameObject.activeSelf)
                    count++;
            }
            GetComponent<Text>().text = "Discard Pile: " + count;
            count = 0;
        }
    }
}