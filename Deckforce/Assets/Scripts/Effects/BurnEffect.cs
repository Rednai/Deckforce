using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Burn")]
public class BurnEffect : Effect
{
    public override void Activate(Entity targetEntity)
    {
        targetEntity.TakeDamage(value);
        base.Activate(targetEntity);
    }
}
