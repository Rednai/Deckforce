using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : Entity
{
    public Player playerOwner;

    public int damage;

    private Pathfinding pathfinding;
    private Range range;
    private BattleManager battleManager;

    private Entity target;
    private List<Tile> pathToTarget;
    private bool onMove = false;
    private Vector3 nextDest;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        pathfinding = GetComponent<Pathfinding>();
        range = GetComponent<Range>();
        battleManager = FindObjectOfType<BattleManager>();
        nextDest = transform.position;
        nextDest.y = 0.5f;
        base.Init();
    }

    private void Update()
    {
        if (onMove)
        {
            if (pathToTarget.Count != 0 && checkCharacterPositionAtDest(nextDest))
            {
                nextDest = pathToTarget[0].GetComponent<Transform>().position;
                nextDest.y = 0.5f;
                if (pathToTarget[0].tileTrap != null)
                    pathToTarget[0].tileTrap.Activate(this);
                pathToTarget.RemoveAt(0);
            }
            else if (pathToTarget.Count == 0 && checkCharacterPositionAtDest(nextDest))
            {
                onMove = false;
                Attack();
            }
        }
    }

    private void FixedUpdate()
    {
        if (onMove & !checkCharacterPositionAtDest(nextDest))
            transform.position = Vector3.MoveTowards(transform.position, nextDest, 0.05f);
    }

    public override void StartTurn()
    {
        base.StartTurn();
        pathToTarget = null;
        List<Entity> targets = new List<Entity>();
        Entity[] entities = FindObjectsOfType<Entity>();
        foreach (Entity elem in entities)
            if (elem != this & elem != playerOwner.selectedCharacter & !playerOwner.selectedCharacter.alliedEntities.Contains(elem) & elem.entityType != EntityType.PROP & elem.entityType != EntityType.TRAP)
                targets.Add(elem);
        foreach (Entity elem in targets)
        {
            Debug.Log(elem.name);
            List<Tile> path = pathfinding.findPathtoCase(elem.GetComponent<Pathfinding>().startTile, true);
            if (path.Count <= 2)
            {
                target = elem;
                Attack();
                return;
            }
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
        if (pathToTarget.Count == 0)
            EndTurn();
        pathToTarget.RemoveAt(pathToTarget.Count - 1);
        for (; pathToTarget.Count - 1 > currentMovePoints; pathToTarget.RemoveAt(pathToTarget.Count - 1));
        pathToTarget[pathToTarget.Count - 1].tileEntity = this;
        pathfinding.startTile.tileEntity = null;
        pathfinding.startTile = pathToTarget[pathToTarget.Count - 1];
        onMove = true;
    }

    public override void EndTurn()
    {
        battleManager.FinishTurn();
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

    private void Attack()
    {
        if (target != null)
            if (pathfinding.findPathtoCase(target.GetComponent<Pathfinding>().startTile, true).Count == 2)
                target.TakeDamage(damage);
        EndTurn();
    }

    private bool checkCharacterPositionAtDest(Vector3 dest)
    {
        if (transform.position.x == dest.x && transform.position.z == dest.z)
            return true;
        return false;
    }
}
