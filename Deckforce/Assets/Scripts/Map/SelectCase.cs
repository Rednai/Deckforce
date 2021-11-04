using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

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


// Custom Editor the "old" way by modifying the script variables directly.
// No handling of multi-object editing, undo, and Prefab overrides!
[CustomEditor (typeof(SelectCase))]
public class SelectCaseEditor : Editor
{
    bool[,] patterns = new bool[10, 10];

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        for (ushort y = 0; y < 10; y++) {
            EditorGUILayout.BeginHorizontal();
            for (ushort x = 0; x < 10; x++) {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.Toggle(patterns[y, x]);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}