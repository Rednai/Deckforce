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

    public override void Init()
    {
        currentActionPoints = maxActionPoints;
        base.Init();
    }

    public void RightClick()
    {}

    public void LeftClick()
    {}

    public override void StartTurn()
    {
        currentActionPoints = maxActionPoints;
        base.StartTurn();
    }

    public override void EndTurn()
    {
        ApplyEffects(Effect.ActivationTime.ENDTURN);
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
    }

    public override void Die()
    {
        battleManager.RemovePlayer(transform.parent.GetComponent<Player>());
        base.Die();
    }

    public void AddEntityToAllies(Entity newEntity)
    {
        newEntity.battleId = battleManager.battleId;
        battleManager.battleId++;
        alliedEntities.Add(newEntity);
    }

    public void RemoveEntityFromAllies(Entity newEntity)
    {
        alliedEntities.Remove(newEntity);
        battleManager.RemoveEntity(newEntity);
    }
}
