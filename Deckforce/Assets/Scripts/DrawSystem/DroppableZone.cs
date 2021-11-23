using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawSystem
{
    public class DroppableZone : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        protected SelectCase floor;

        public void OnDrop(PointerEventData eventData)
        {
            if (floor.currentSelected != null)
            {
                Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
                CardDisplay cardDisplay = draggable.GetComponent<CardDisplay>();

                bool isActivated = cardDisplay.card.Activate(cardDisplay.ownerPlayer, floor.currentSelected);
                if (draggable != null && isActivated == true) {
                    draggable.parentToReturnTo = draggable.discardPile.transform;
                }
                Debug.Log(eventData.pointerDrag.name + " was dropped to " + floor.currentSelected.gameObject.name);
            }
        }
    }
}
