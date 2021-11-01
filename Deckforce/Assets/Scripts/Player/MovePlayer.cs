using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SelectCase floor;

    private Character character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (select.currentSelected != null && Input.GetMouseButtonDown(0) && character.canMove) {
            Vector3 casePos = select.currentSelected.GetComponentInParent<Transform>().position;
            int distance = (int)Mathf.Abs((casePos - character.transform.position).magnitude);
            Debug.Log(casePos);
            Debug.Log(distance);
            int movementCost = 1 + distance/2;

            if (character.currentMovePoints >= movementCost) {
                casePos.y += 0.5f;
                character.transform.position = casePos;
                character.currentMovePoints -= movementCost;
            }
        }
    }
}
