using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressionCard : Card
{
    public int damage;

    public override void Activate(Player currentPlayer, Tile targetTile)
    {
        Entity targetEntity = targetTile.tileEntity;

        if (targetEntity) {
            targetEntity.TakeDamge(damage);
        }
        currentPlayer.selectedCharacter.currentActionPoints -= cost;
    }
}
