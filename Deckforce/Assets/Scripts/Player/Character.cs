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

    public void RightClick()
    {}

    public void LeftClick()
    {}

    public void StartTurn()
    {
        currentMovePoints = maxMovePoints;
        currentActionPoints = maxActionPoints;
    }

    public void AddEntityToAllies(Entity newEntity)
    {
        alliedEntities.Add(newEntity);
    }

    public void RemoveEntityFromAllies(Entity newEntity)
    {
        alliedEntities.Remove(newEntity);
    }
}
