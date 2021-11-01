using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    int currentMovePoints;
    public int maxMovePoints;
    int currentActionPoints;
    public int maxActionPoints;

    public void RightClick()
    {}

    public void LeftClick()
    {}

    public void StartTurn()
    {
        currentMovePoints = maxMovePoints;
        currentActionPoints = maxActionPoints;
    }
}
