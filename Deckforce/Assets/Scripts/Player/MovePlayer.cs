using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SelectCase floor;

    private Character character;

    public bool onMove = false;
    private Pathfinding pathfinding;

    private List<Tile> move = new List<Tile>();
    private Vector3 nextDest;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
        pathfinding = GetComponent<Pathfinding>();
        nextDest = character.transform.position;
        nextDest.y = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Tile currentSelected = floor.currentSelected;
        if (move.Count == 0 && currentSelected != null && currentSelected.tileEntity == null && Input.GetMouseButtonDown(0) && character.canMove) {
            List<Tile> path = pathfinding.findPathtoCase(currentSelected);
            Vector3 casePos = currentSelected.GetComponentInParent<Transform>().position;
            int movementCost = path.Count - 1;

            if (character.currentMovePoints >= movementCost)
            {
                move = path;
                character.currentMovePoints -= movementCost;
                pathfinding.setStartTile(currentSelected);
            }
        }

        if (move.Count != 0 && checkCharacterPositionAtDest(nextDest))
        {
            nextDest = move[0].GetComponent<Transform>().position;
            nextDest.y = 0.5f;
            move.RemoveAt(0);
        }

        if (!checkCharacterPositionAtDest(nextDest))
        {
            character.transform.position = Vector3.MoveTowards(transform.position, nextDest, 0.01f);
        }
    }

    private bool checkCharacterPositionAtDest(Vector3 dest)
    {
        if (character.transform.position.x == dest.x && character.transform.position.z == dest.z)
            return true;
        return false;
    }
}
