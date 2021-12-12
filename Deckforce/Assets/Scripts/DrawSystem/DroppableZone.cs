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

        GameServer gameServer;
        public Range range;

        void Start()
        {
            gameServer = GameObject.FindObjectOfType<GameServer>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            CardDisplay cardDisplay = draggable.GetComponent<CardDisplay>();
            List<Tile> playerRange = range.GetRangeTiles(cardDisplay.ownerPlayer.selectedCharacter.GetComponent<Pathfinding>().startTile, cardDisplay.card.areaTypePattern, cardDisplay.card.playerRange, true, false);
            if (floor.currentSelected != null & playerRange.Contains(floor.currentSelected)) {

                if (!cardDisplay.ownerPlayer.isClient) {
                    return ;
                }

                bool isActivated = cardDisplay.card.Activate(cardDisplay.ownerPlayer, floor.currentSelected);
                if (draggable != null && isActivated == true) {
                    draggable.parentToReturnTo = draggable.discardPile.transform;
                    ActivateCard activateCard = new ActivateCard();
                    activateCard.cardId = cardDisplay.card.id;
                    activateCard.playerId = cardDisplay.ownerPlayer.id;
                    activateCard.tileName = floor.currentSelected.transform.name;
                    gameServer.SendData(activateCard);
                }
                Debug.Log(eventData.pointerDrag.name + " was dropped to " + floor.currentSelected.gameObject.name);
            }
        }
    }
}
