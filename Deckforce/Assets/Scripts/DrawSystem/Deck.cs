using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DrawSystem
{
    public class Deck : MonoBehaviour
    {
        [SerializeField]public GameObject cardPrefab;
        public GameObject hand;
        private Draggable card;
        private bool isDrawing = false;
        private float duration = 1f;
        GameObject tempCardTarget;
        
        IEnumerator Drawing() 
        {
            isDrawing = false;
            tempCardTarget = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempCardTarget.transform.SetParent(hand.transform);
            tempCardTarget.transform.localScale = new Vector3(0f, 0f, 0f);
            
            card.transform.localScale = new Vector3(2f, 2f, 2f);
            for (float t = 0f; t < duration; t += Time.deltaTime) {
                card.transform.position =
                    Vector3.Lerp(card.transform.position, tempCardTarget.transform.position, t*duration);
                yield return null;
            }
            card.transform.localScale = new Vector3(1f, 1f, 1f);
            card.parentToReturnTo = hand.transform.parent;
            Destroy(tempCardTarget);
            card.transform.SetParent(hand.transform);

        }
        public void Draw()
        {
            card = this.transform.GetChild(Random.Range(1, this.transform.childCount)).GetComponent<Draggable>();
            isDrawing = true;
        }

        public void FixedUpdate()
        {
            if (isDrawing)
            {
                StartCoroutine(Drawing());
            }
        }
    }
}
