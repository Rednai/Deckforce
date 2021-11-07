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
                Card card = draggable.GetComponent<CardDisplay>().card;

                if (draggable != null) {
                    draggable.parentToReturnTo = draggable.discardPile.transform;
                }

                card.Activate(card.playerOwner, floor.currentSelected);
                Debug.Log(eventData.pointerDrag.name + " was dropped to " + floor.currentSelected.gameObject.name);
            }
        }
    }
}
