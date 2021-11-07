using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public int currentMovePoints;
    public int maxMovePoints;
    public int currentActionPoints;
    public int maxActionPoints;

    public List<Entity> alliedEntities;

    BattleManager battleManager;

    void Start()
    {
        battleManager = GameObject.FindObjectOfType<BattleManager>();
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
        Debug.Log("take damage");
        base.TakeDamage(damageAmount);
    }

    public override void Die()
    {
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
        //TODO: retirer aussi du battleturn
        battleManager.RemoveEntity(newEntity);
    }
}
