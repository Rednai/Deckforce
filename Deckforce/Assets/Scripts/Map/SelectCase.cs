using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectCase: MonoBehaviour {
    public LayerMask floorMask;
    public Tile currentSelected = null;
    public bool spawningMode = true;
    private bool spawn = true;

    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorMask)) {
            if (currentSelected != null & spawningMode) {
                currentSelected.StopAnimation();
            }
            currentSelected = hit.transform.parent.GetComponent<Tile>();
            if (spawningMode)
                currentSelected.StartAnimation();
        } else if (currentSelected != null & spawningMode) {
            currentSelected.StopAnimation();
            currentSelected = null;
        } else if (currentSelected != null) {
            currentSelected.StopAnimation();
            currentSelected = null;
        }
        if (spawn != spawningMode)
            currentSelected.StopAnimation();
        spawn = spawningMode;
    }
}