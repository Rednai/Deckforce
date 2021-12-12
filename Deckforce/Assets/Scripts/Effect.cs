using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Effect
{
    public enum ActivationTime { STARTTURN, ENDTURN };
    public ActivationTime activationTime;

    public enum TargetType { SELF, TARGET };
    public TargetType targetType;

    public enum EffectType { SLOW, BURN };
    public EffectType effectType;

    public int value;

    public int maxTurns;
    public int remainingTurns;

    public void Activate(Entity targetEntity)
    {
        switch (effectType) {
            case (EffectType.SLOW):
                targetEntity.currentMovePoints -= value;
                break;
            case (EffectType.BURN):
                targetEntity.currentLife -= value;
                break;
        }
        remainingTurns--;
        if (remainingTurns == 0) {
            targetEntity.appliedEffects.Remove(this);
        }
    }
}
