using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SelectCase select;

    private Transform character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (select.currentSelected != null & Input.GetMouseButtonDown(0))
        {
            Vector3 casePos = select.currentSelected.GetComponentInParent<Transform>().position;
            Debug.Log(casePos);
            casePos.y += 0.5f;
            character.position = casePos;
        }
    }
}
