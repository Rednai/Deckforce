using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawSystem
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Transform parentToReturnTo = null;
        public GameObject discardPile;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            DroppableZone[] dropZones = GameObject.FindObjectsOfType<DroppableZone>();
            // TODO: Use these dropzones to make them glow
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo);
            
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            DroppableZone[] dropZones = GameObject.FindObjectsOfType<DroppableZone>();
            // TODO: Use these dropzones to make them NOT glow
        }
    }
}
