using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DrawSystem
{
    public class DroppableZone : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        protected SelectCase floor;

        public Range range;

        public void OnDrop(PointerEventData eventData)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            CardDisplay cardDisplay = draggable.GetComponent<CardDisplay>();
            List<Tile> playerRange = range.GetRangeTiles(cardDisplay.ownerPlayer.selectedCharacter.GetComponent<Pathfinding>().startTile, RangeType.MOVEMENT, cardDisplay.card.playerRange, true, false);
            if (floor.currentSelected != null & playerRange.Contains(floor.currentSelected))
            {
                bool isActivated = cardDisplay.card.Activate(cardDisplay.ownerPlayer, floor.currentSelected);
                if (draggable != null && isActivated == true) {
                    draggable.parentToReturnTo = draggable.discardPile.transform;
                }
                Debug.Log(eventData.pointerDrag.name + " was dropped to " + floor.currentSelected.gameObject.name);
            }
        }
    }
}
