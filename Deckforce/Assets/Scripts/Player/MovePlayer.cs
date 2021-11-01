using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SelectCase floor;

    private Transform character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (floor.currentSelected != null & Input.GetMouseButtonDown(0))
        {
            Vector3 casePos = floor.currentSelected.GetComponentInParent<Transform>().position;
            casePos.y += 0.5f;
            character.position = casePos;
        }
    }
}
