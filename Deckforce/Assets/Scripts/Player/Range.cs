using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    CROSS,
    SQUARE,
    MOVEMENT
}

public class Range : MonoBehaviour
{

    public List<Tile> GetRangeTiles(Tile startTile, RangeType rType, int size, bool targetEntity, bool blockByEntity)
    {
        List<Tile> range = new List<Tile>();

        switch (rType)
        {
            case (RangeType.MOVEMENT):
                range = GetMovementRange(startTile, size, targetEntity, blockByEntity, range);
                break;
        }
        return range;
    }

    private List<Tile> GetMovementRange(Tile startTile, int size, bool targetEntity, bool blockByEntity, List<Tile> range)
    {
        List<Tile> ignored = new List<Tile>();
        List<Tile> frontier = new List<Tile>();
        frontier.Add(startTile);
        int count = 0;
        
        while (frontier.Count > 0)
        {
            Tile current = frontier[0];
            List<Tile> neighborgs = GetNextTile(current);
            int rangeSize = GetRangeFromStart(startTile.tilePosition, current.tilePosition);

            if (size > rangeSize & (current.tileEntity == null | !blockByEntity | current == startTile))
                foreach (Tile next in neighborgs)
                    if (!range.Contains(next) & !ignored.Contains(next))
                        frontier.Add(next);

            if (!range.Contains(current) & !ignored.Contains(current))
            {
                if ((!targetEntity & current.tileEntity == null) | targetEntity)
                    range.Add(current);
                else
                    ignored.Add(current);   
            }
            frontier.RemoveAt(0);
            count += 1;
        }
        if (range.Count <= 1)
            return new List<Tile>();
        return range;
    }

    private int GetRangeFromStart(Vector2 start, Vector2 target)
    {
        return (int) (Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y));
    }

    private List<Tile> GetNextTile(Tile current)
    {
        List<Tile> next = new List<Tile>();

        Tile border = current.GetRelatedPos(RelatedPos.UP);
        if (border != null)
            next.Add(border);

        border = current.GetRelatedPos(RelatedPos.LEFT);
        if (border != null)
            next.Add(border);

        border = current.GetRelatedPos(RelatedPos.DOWN);
        if (border != null)
            next.Add(border);

        border = current.GetRelatedPos(RelatedPos.RIGHT);
        if (border != null)
            next.Add(border);
        return next;
    }

    public void HighlightRange(List<Tile> range, OutlineType type)
    {
        foreach (Tile elem in range)
            elem.SetOutline(type);
    }

    public void CancelHighlightRange(List<Tile> range)
    {
        foreach (Tile elem in range)
            elem.StopOutline();
    }
}
