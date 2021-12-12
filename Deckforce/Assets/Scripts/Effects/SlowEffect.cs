using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Slow")]
public class SlowEffect : Effect
{
    public override void Activate(Entity targetEntity)
    {
        Debug.Log("slow");

        targetEntity.currentMovePoints -= value;
        base.Activate(targetEntity);
    }
}
