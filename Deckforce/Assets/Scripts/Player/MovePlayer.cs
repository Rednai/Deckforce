using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public SelectCase floor;

    private Character character;

    public bool onMove = false;
    public Pathfinding pathfinding;
    private Range range;

    private List<Tile> pathToCurrentSelected = new List<Tile>();
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
        if (move.Count == 0 && currentSelected != null && currentSelected.tileEntity == null && character.canMove) {
            List<Tile> path = pathfinding.findPathtoCase(currentSelected);
            if (path != pathToCurrentSelected)
            {
                cancelPathAnimation(pathToCurrentSelected);
                pathToCurrentSelected = path;
                if (path.Count - 1 <= character.currentMovePoints & path.Count > 1)
                {
                    animatePath(pathToCurrentSelected);
                }
            }
            if (Input.GetMouseButtonDown(0)) {
                MoveCharacter(currentSelected, path);
            }   
        }

        if (character.canMove)
        {
            range.CancelHighlightRange(highlightedRange);
            highlightedRange = range.GetRangeTiles(pathfinding.startTile, RangeType.MOVEMENT, character.currentMovePoints, false, true);
            range.HighlightRange(highlightedRange, OutlineType.MOVE);
        } else if (highlightedRange.Count > 0)
        {
            range.CancelHighlightRange(highlightedRange);
            cancelPathAnimation(pathToCurrentSelected);
            highlightedRange = new List<Tile>();
            pathToCurrentSelected = new List<Tile>();
        }

        if (move.Count != 0 && checkCharacterPositionAtDest(nextDest))
        {
            
            nextDest = move[0].GetComponent<Transform>().position;
            nextDest.y = 0.5f;
            Debug.Log(character.transform.position + " " + nextDest + " " + checkCharacterPositionAtDest(nextDest));
            move.RemoveAt(0);
        }

        if (!checkCharacterPositionAtDest(nextDest))
        {
            character.transform.position = Vector3.MoveTowards(transform.position, nextDest, 0.01f);
            
        }
    }

    public void MoveCharacter(Tile currentSelected, List<Tile> path)
    {
        Vector3 casePos = currentSelected.GetComponentInParent<Transform>().position;
        int movementCost = path.Count - 1;

        if (movementCost > 0 && character.currentMovePoints >= movementCost) {
            move = path;
            character.currentMovePoints -= movementCost;
            pathfinding.startTile.tileEntity = null;
            pathfinding.setStartTile(currentSelected);
            currentSelected.SetEntity(character);
            cancelPathAnimation(pathToCurrentSelected);
            pathToCurrentSelected = new List<Tile>();
        }
    }

    private bool checkCharacterPositionAtDest(Vector3 dest)
    {
        if (character.transform.position.x == dest.x && character.transform.position.z == dest.z)
            return true;
        return false;
    }

    private void animatePath(List<Tile> path)
    {
        foreach (Tile elem in path)
        {
            elem.StartAnimation();
        }
    }

    private void cancelPathAnimation(List<Tile> path)
    {
        foreach (Tile elem in path)
        {
            elem.StopAnimation();
        }
    }
}
