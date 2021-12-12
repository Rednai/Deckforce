using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public int currentActionPoints;
    public int maxActionPoints;

    public List<Entity> alliedEntities;

    public BattleManager battleManager;

    void Start()
    {
        Init();
    }

    public void RightClick()
    {}

    public void LeftClick()
    {}

    public override void StartTurn()
    {
        currentMovePoints = maxMovePoints;
        currentActionPoints = maxActionPoints;
        canMove = true;
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
    }

    public override void Die()
    {
        Player characterPlayer = transform.parent.GetComponent<Player>();

        battleManager.RemovePlayer(transform.parent.GetComponent<Player>());
        base.Die();
    }

    public void AddEntityToAllies(Entity newEntity)
    {
        alliedEntities.Add(newEntity);
    }

    public void RemoveEntityFromAllies(Entity newEntity)
    {
        alliedEntities.Remove(newEntity);
        battleManager.RemoveEntity(newEntity);
    }
}
