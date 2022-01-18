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

        public AudioClip cannotClip;

        public void OnDrop(PointerEventData eventData)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            CardDisplay cardDisplay = draggable.GetComponent<CardDisplay>();
            List<Tile> playerRange = range.GetRangeTiles(cardDisplay.ownerPlayer.selectedCharacter.GetComponent<Pathfinding>().startTile, cardDisplay.card.areaTypePattern, cardDisplay.card.playerRange, cardDisplay.card.targetEntity, cardDisplay.card.areablockByEntity);
            if (floor.currentSelected != null & playerRange.Contains(floor.currentSelected)) {

                if (!cardDisplay.ownerPlayer.isClient) {
                    return ;
                }

                List<Tile> targetsTiles = range.GetRangeTiles(floor.currentSelected, cardDisplay.card.effectTypePattern, cardDisplay.card.effectRange, cardDisplay.card.targetEntity, cardDisplay.card.effectblockByEntity);

                bool isActivated = cardDisplay.card.Activate(cardDisplay.ownerPlayer, targetsTiles, floor.currentSelected);
                if (draggable != null && isActivated == true) {
                    draggable.parentToReturnTo = draggable.discardPile.transform;
                    if (!GameServer.instance.isOffline) {
                        ActivateCard activateCard = new ActivateCard();
                        activateCard.cardId = cardDisplay.card.id;
                        activateCard.playerId = cardDisplay.ownerPlayer.id;
                        activateCard.tileName = floor.currentSelected.transform.name;
                        GameServer.instance.SendData(activateCard);
                    }
                }
                Debug.Log(eventData.pointerDrag.name + " was dropped to " + floor.currentSelected.gameObject.name);
            }
            else
            {
                SoundsManager.instance.PlaySound(cannotClip);
            }
        }
    }
}
