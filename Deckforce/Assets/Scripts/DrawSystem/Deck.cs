using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DrawSystem
{
    public class Deck : MonoBehaviour
    {
        public GameObject hand;
        private Draggable card;
        private bool isDrawing = false;
        private float duration = 1f;
        
        IEnumerator Drawing() 
        {
            isDrawing = false;
            for (float t = 0f; t < duration; t += Time.deltaTime) {
                card.transform.position =
                    Vector3.Lerp(card.transform.position, hand.transform.position, t*duration);
                yield return null;
            }
            Debug.Log("LerpFinished");
            card.parentToReturnTo = hand.transform.parent;
            Debug.Log("SettingParent");
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
