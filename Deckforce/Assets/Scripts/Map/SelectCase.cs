using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCase: MonoBehaviour {
    public LayerMask floorMask;
    public GameObject currentSelected = null;

    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorMask)) {
            if (currentSelected != null) {
                currentSelected.GetComponent<Animator>().SetBool("Selected", false);
            }
            currentSelected = hit.transform.parent.gameObject;
            currentSelected.GetComponent<Animator>().SetBool("Selected", true);
        } else if (currentSelected != null) {
            currentSelected.GetComponent<Animator>().SetBool("Selected", false);
            currentSelected = null;
        }
    }
}
