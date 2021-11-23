using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : Entity
{
    public Player playerOwner;

    public override void StartTurn()
    {
        //TODO: finir le tour instant tant qu'on a pas de gestion d'IA
    }

    public override void EndTurn()
    {
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
}