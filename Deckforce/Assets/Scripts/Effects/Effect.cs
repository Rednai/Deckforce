using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect")]
public class Effect : ScriptableObject
{
    public enum ActivationTime { STARTTURN, ENDTURN };
    public ActivationTime activationTime;

    public enum TargetType { SELF, TARGET };
    public TargetType targetType;

    public int value;

    public int maxTurns;
    public int remainingTurns;

    public Sprite effectIcon;

    public virtual void Activate(Entity targetEntity)
    {
        remainingTurns--;
    }
}
