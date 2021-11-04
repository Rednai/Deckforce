using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawSystem
{
    public class DroppableZone : MonoBehaviour, IDropHandler
    {
        [SerializeField]protected SelectCase floor;
        public void OnDrop(PointerEventData eventData)
        {
            if (floor.currentSelected != null)
            {
                Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
                if (d != null)
                {
                    d.parentToReturnTo = d.discardPile.transform;
                }
                Debug.Log(eventData.pointerDrag.name + " was dropped to " + floor.currentSelected.gameObject.name);
            }
        }
    }
}
