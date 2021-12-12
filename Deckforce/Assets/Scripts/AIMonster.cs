using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : Entity
{
    public Player playerOwner;

    private Pathfinding pathfinding;
    private Range range;

    private Entity target;
    private List<Tile> pathToTarget;
    private bool onTurn = false;
    private Vector3 nextDest;


    private void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        range = GetComponent<Range>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public override void StartTurn()
    {
        pathToTarget = null;
        List<Entity> targets = new List<Entity>();
        Entity[] entities = FindObjectsOfType<Entity>();
        foreach (Entity elem in entities)
            if (elem != this & elem != playerOwner.selectedCharacter & !playerOwner.selectedCharacter.alliedEntities.Contains(elem))
                targets.Add(elem);
        foreach (Entity elem in targets)
        {
            List<Tile> path = pathfinding.findPathtoCase(elem.GetComponent<Pathfinding>().startTile, true);
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
        pathToTarget.RemoveAt(pathToTarget.Count - 1);
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
