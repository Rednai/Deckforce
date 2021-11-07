using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Tile startTile = null;

    private void getStartTile()
    {
        Vector3 pos = GetComponent<Transform>().position;
        Debug.Log(pos);
        startTile = GameObject.Find($"Case x:{pos.z} y:{pos.x}").GetComponent<Tile>();
    }

    public void setStartTile(Tile tile)
    {
        startTile = tile;
    }

    public List<Tile> findPathtoCase(Tile goal)
    {
        List<Tile> path = new List<Tile>();
        List<Tile> frontier = new List<Tile>();
        Dictionary<Tile, Tile> reached = new Dictionary<Tile, Tile>();

        frontier.Add(startTile);
        reached.Add(startTile, null);

        int c = 0;

        while (frontier.Count != 0)
        {
            Tile current = frontier[0];
            frontier.RemoveAt(0);
            List<Tile> neighbors = getFrontierTile(current);

            foreach (Tile next in neighbors)
            {
                if (!reached.ContainsKey(next))
                {
                    frontier.Add(next);
                    reached.Add(next, current);
                }
            }
            c += 1;
        }

        Tile actual = goal;

        while (actual != startTile)
        {
            path.Add(actual);
            actual = reached[actual];
        }
        path.Add(startTile);
        path.Reverse();
        
        return path;
    }

    private List<Tile> getFrontierTile(Tile current)
    {
        List<Tile> next = new List<Tile>();

        Tile border = current.GetRelatedPos(RelatedPos.UP);
        if (border != null)
            if (border.tileEntity == null)
                next.Add(border);

        border = current.GetRelatedPos(RelatedPos.LEFT);
        if (border != null)
            if (border.tileEntity == null)
                next.Add(border);

        border = current.GetRelatedPos(RelatedPos.DOWN);
        if (border != null)
            if (border.tileEntity == null)
                next.Add(border);

        border = current.GetRelatedPos(RelatedPos.RIGHT);
        if (border != null)
            if (border.tileEntity == null)
                next.Add(border);
        return next;
    }
}
