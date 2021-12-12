using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace DrawSystem
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform parentToReturnTo = null;
        public GameObject discardPile;
        private Canvas tempCanvas;
        private GraphicRaycaster tempRaycaster;
        public Range range;
        private SelectCase floor;
        private List<Tile> highlightedEffect = new List<Tile>();
        private List<Tile> highlightedRange = new List<Tile>();
        private Tile current = null;
        private CardDisplay card;

        public AudioClip cardHoverClip;

        public void OnBeginDrag(PointerEventData eventData)
        {
            floor = FindObjectsOfType<SelectCase>()[0];
            card = GetComponent<CardDisplay>();
            parentToReturnTo = this.transform.parent;
            discardPile = GameObject.Find("DiscardPile");
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            this.transform.Rotate(new Vector3(0f, 0f, 40f));
            card.ownerPlayer.selectedCharacter.GetComponent<MovePlayer>().StopMoveMode();

            GetComponent<CanvasGroup>().blocksRaycasts = false;

            DroppableZone[] dropZones = GameObject.FindObjectsOfType<DroppableZone>();
            // TODO: Use these dropzones to make them glow

            SoundsManager.instance.PlaySound(card.selectClip, 1f);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = new Vector2(eventData.position.x + 40, eventData.position.y + 50);


            range.CancelHighlightRange(highlightedRange);
            highlightedRange = range.GetRangeTiles(card.ownerPlayer.selectedCharacter.GetComponent<Pathfinding>().startTile, card.card.areaTypePattern, card.card.playerRange, card.card.targetEntity, card.card.areablockByEntity);
            range.HighlightRange(highlightedRange, OutlineType.RANGE);


            if (current != floor.currentSelected | current == null)
            {
                cancelZoneAnimation(highlightedEffect);
                range.CancelHighlightRange(highlightedEffect);
                highlightedEffect = new List<Tile>();
            }

            List<Tile> effects = range.GetRangeTiles(floor.currentSelected, card.card.effectTypePattern, card.card.effectRange, card.card.targetEntity, card.card.effectblockByEntity);
            if (effects != highlightedEffect & highlightedRange.Contains(floor.currentSelected))
            {
                cancelZoneAnimation(highlightedEffect);
                range.CancelHighlightRange(highlightedEffect);
                highlightedEffect = effects;
                animateZone(highlightedEffect);
            }
            range.HighlightRange(highlightedEffect, OutlineType.EFFECT);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            DroppableZone[] dropZones = GameObject.FindObjectsOfType<DroppableZone>();
            cancelZoneAnimation(highlightedEffect);
            range.CancelHighlightRange(highlightedEffect);
            highlightedEffect = new List<Tile>();
            range.CancelHighlightRange(highlightedRange);
            highlightedRange = new List<Tile>();
            // TODO: Use these dropzones to make them NOT glow

            if (parentToReturnTo == transform.parent && !card.card.isActivated) {
                SoundsManager.instance.PlaySound(card.selectBackClip, 1f);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tempCanvas = gameObject.AddComponent<Canvas>();
            tempCanvas.overrideSorting = true;
            tempCanvas.sortingOrder = 1;
            tempRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            
            transform.localScale = new Vector3(2f, 2f, 2f);
            SoundsManager.instance.PlaySound(cardHoverClip, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(tempRaycaster);
            Destroy(tempCanvas);
            
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void animateZone(List<Tile> path)
        {
            foreach (Tile elem in path)
            {
                elem.StartAnimation();
            }
        }

        private void cancelZoneAnimation(List<Tile> path)
        {
            foreach (Tile elem in path)
            {
                elem.StopAnimation();
            }
        }
    }
}
