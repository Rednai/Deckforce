using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawSystem
{
    public class DroppableZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log(eventData.pointerDrag.name + " was dropped to " + gameObject.name);
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
            {
                d.parentToReturnTo = d.discardPile.transform;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}
