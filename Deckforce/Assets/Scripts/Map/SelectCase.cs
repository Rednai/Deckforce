using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectCase: MonoBehaviour {
    public LayerMask floorMask;
    public Tile currentSelected = null;

    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorMask)) {
            if (currentSelected != null) {
                currentSelected.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.UP)?.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.DOWN)?.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.LEFT)?.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.StopAnimation();
                currentSelected.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.UP)?.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.DOWN)?.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.LEFT)?.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.StopOutline();
            }
            currentSelected = hit.transform.GetComponent<Tile>();
                currentSelected.StartAnimation();
                currentSelected.GetRelatedPos(RelatedPos.UP)?.StartAnimation();
                currentSelected.GetRelatedPos(RelatedPos.DOWN)?.StartAnimation();
                currentSelected.GetRelatedPos(RelatedPos.LEFT)?.StartAnimation();
                currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.StartAnimation();
                currentSelected.SetOutline(OutlineType.MOVE);
                currentSelected.GetRelatedPos(RelatedPos.UP)?.SetOutline(OutlineType.MOVE);
                currentSelected.GetRelatedPos(RelatedPos.DOWN)?.SetOutline(OutlineType.MOVE);
                currentSelected.GetRelatedPos(RelatedPos.LEFT)?.SetOutline(OutlineType.MOVE);
                currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.SetOutline(OutlineType.MOVE);
        } else if (currentSelected != null) {
                currentSelected.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.UP)?.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.DOWN)?.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.LEFT)?.StopAnimation();
                currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.StopAnimation();
                currentSelected.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.UP)?.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.DOWN)?.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.LEFT)?.StopOutline();
                currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.StopOutline();
            currentSelected = null;
        }

        // Debug, aucune utilité
        if (Input.GetMouseButtonDown(0) && currentSelected != null) {
            //Debug.Log($"Current tile: {currentSelected.name} T: {currentSelected.GetRelatedPos(RelatedPos.UP)?.name} D: {currentSelected.GetRelatedPos(RelatedPos.DOWN)?.name} L: {currentSelected.GetRelatedPos(RelatedPos.LEFT)?.name} R: {currentSelected.GetRelatedPos(RelatedPos.RIGHT)?.name}");
        }
    }
}