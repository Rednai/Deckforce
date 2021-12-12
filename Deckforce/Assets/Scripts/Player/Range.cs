using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum RangeType
{
    SQUARE,
    CIRCULAR, 
    CROSS,
    SINGLE,
    DIAGONAL,
    CROSS_DIAGONAL
}

public class Range : MonoBehaviour
{
    private List<RelatedPos> cross = new List<RelatedPos> { RelatedPos.UP, RelatedPos.DOWN, RelatedPos.LEFT, RelatedPos.RIGHT };
    private List<RelatedPos> diagonal = new List<RelatedPos> { RelatedPos.UP_LEFT, RelatedPos.UP_RIGHT, RelatedPos.DOWN_LEFT, RelatedPos.DOWN_RIGHT };
    private List<RelatedPos> cross_diagonal = new List<RelatedPos> { RelatedPos.UP, RelatedPos.DOWN, RelatedPos.LEFT, RelatedPos.RIGHT, RelatedPos.UP_LEFT, RelatedPos.UP_RIGHT, RelatedPos.DOWN_LEFT, RelatedPos.DOWN_RIGHT };

    public List<Tile> GetRangeTiles(Tile startTile, RangeType rType, int size, bool targetEntity, bool blockByEntity)
    {
        List<Tile> range = new List<Tile>();

        switch (rType)
        {
            case (RangeType.CIRCULAR):
                range = GetCircularRange(startTile, size, targetEntity, blockByEntity, range, cross);
                break;
            case (RangeType.SINGLE):
                range = GetSingleRange(startTile, targetEntity, range);
                break;
            case (RangeType.CROSS):
                range = GetCrossRange(startTile, size, targetEntity, blockByEntity, range, cross);
                break;
            case (RangeType.DIAGONAL):
                range = GetCrossRange(startTile, size, targetEntity, blockByEntity, range, diagonal);
                break;
            case (RangeType.CROSS_DIAGONAL):
                range = GetCrossRange(startTile, size, targetEntity, blockByEntity, range, cross_diagonal);
                break;
            case (RangeType.SQUARE):
                range = GetCircularRange(startTile, size, targetEntity, blockByEntity, range, cross_diagonal);
                break;
        }
        return range;
    }

    private List<Tile> GetCircularRange(Tile startTile, int size, bool targetEntity, bool blockByEntity, List<Tile> range, List<RelatedPos> pos)
    {
        List<Tile> ignored = new List<Tile>();
        List<Tile> frontier = new List<Tile>();
        frontier.Add(startTile);

        if (startTile == null)
            return new List<Tile>();

        while (frontier.Count > 0)
        {
            Tile current = frontier[0];
            List<Tile> neighborgs = GetNextTile(current, pos);
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
        }
        if (range.Count <= 1)
            return new List<Tile>();
        return range;
    }

    private List<Tile> GetSingleRange(Tile startTile, bool targetEntity, List<Tile> range)
    {
        if (startTile != null)
            if ((targetEntity & startTile.tileEntity != null) | (!targetEntity & startTile.tileEntity == null))
                range.Add(startTile);
        return range;
    }

    private List<Tile> GetCrossRange(Tile startTile, int size, bool targetEntity, bool blockByEntity, List<Tile> range, List<RelatedPos> listPos)
    {
        if (startTile == null | size <= 0)
            return new List<Tile>();
        range.Add(startTile);

        foreach (RelatedPos p in listPos)
        {
            Tile current = startTile;
            for (int i = 0; i != size - 1; i++)
            {
                Tile next = current.GetRelatedPos(p);
                if (next != null)
                {
                    if (!targetEntity & next.tileEntity != null)
                        current = next;
                    else if (blockByEntity & next.tileEntity != null)
                    {
                        range.Add(next);
                        break;
                    } else
                        range.Add(next);
                }
                else
                    break;
                current = next;
            }
        }
        return range;
    }

    private int GetRangeFromStart(Vector2 start, Vector2 target)
    {
        return (int) (Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y));
    }

    private List<Tile> GetNextTile(Tile current, List<RelatedPos> pos)
    {
        List<Tile> next = new List<Tile>();

        foreach (RelatedPos p in pos)
        {
            Tile border = current.GetRelatedPos(p);
            if (border != null)
                next.Add(border);
        }
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
