using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DrawSystem
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform parentToReturnTo = null;
        public GameObject discardPile;
        private Canvas tempCanvas;
        private GraphicRaycaster tempRaycaster;
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentToReturnTo = this.transform.parent;
            discardPile = GameObject.Find("DiscardPile");
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            this.transform.Rotate(new Vector3(0f, 0f, 40f));

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            DroppableZone[] dropZones = GameObject.FindObjectsOfType<DroppableZone>();
            // TODO: Use these dropzones to make them glow
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = new Vector2(eventData.position.x + 40, eventData.position.y + 50);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            DroppableZone[] dropZones = GameObject.FindObjectsOfType<DroppableZone>();
            // TODO: Use these dropzones to make them NOT glow
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tempCanvas = gameObject.AddComponent<Canvas>();
            tempCanvas.overrideSorting = true;
            tempCanvas.sortingOrder = 1;
            tempRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            
            this.transform.localScale = new Vector3(2f, 2f, 2f);
        
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(tempRaycaster);
            Destroy(tempCanvas);
            
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
