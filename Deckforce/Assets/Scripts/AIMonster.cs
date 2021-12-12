using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : Entity
{
    public Player playerOwner;

    private Pathfinding pathfinding;
    private Range range;

    public Entity target;
    private List<Tile> pathToTarget;
    private bool onTurn = false;

    public List<Entity> targets;

    private void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        range = GetComponent<Range>();
    }

    public override void StartTurn()
    {
        pathToTarget = null;
        targets = new List<Entity>();
        Entity[] entities = FindObjectsOfType<Entity>();
        foreach (Entity elem in entities)
            if (elem != this & elem != playerOwner.selectedCharacter & !playerOwner.selectedCharacter.alliedEntities.Contains(elem))
                targets.Add(elem);
        foreach (Entity elem in targets)
        {
            List<Tile> path = pathfinding.findPathtoCase(elem.GetComponent<Pathfinding>().startTile);
            if (pathToTarget == null)
            {
                target = elem;
                pathToTarget = path;
            } else if (pathToTarget.Count > path.Count)
            {
                target = elem;
                pathToTarget = path;
            }
        }
    }

    public override void EndTurn()
    {
        ;
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
    }

    public override void Die()
    {
        playerOwner.selectedCharacter.RemoveEntityFromAllies(this);
        base.Die();
    }
}
