using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SelectCase floor;

    private Character character;

    public bool onMove = false;
    private Pathfinding pathfinding;
    private Range range;

    private List<Tile> move = new List<Tile>();
    private Vector3 nextDest;

    private List<Tile> highlightedRange = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.FindObjectOfType<SelectCase>();
        character = GetComponent<Character>();
        pathfinding = GetComponent<Pathfinding>();
        range = GetComponent<Range>();
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

            if (movementCost > 0 && character.currentMovePoints >= movementCost)
            {
                move = path;
                character.currentMovePoints -= movementCost;
                pathfinding.startTile.tileEntity = null;
                pathfinding.setStartTile(currentSelected);
                currentSelected.tileEntity = character;
            }
        }

        if (character.canMove)
        {
            range.CancelHighlightRange(highlightedRange);
            highlightedRange = range.GetRangeTiles(pathfinding.startTile, RangeType.MOVEMENT, character.currentMovePoints, false, true);
            range.HighlightRange(highlightedRange, OutlineType.MOVE);
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
