using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Aggression Card")]
public class AggressionCard : Card
{
    public enum ExplosionType { SINGLETILE, SQUARE, ARC };
    public ExplosionType explosionType;
    public int damage;

    public override void Activate(Player currentPlayer, Tile targetTile)
    {
        Entity targetEntity = targetTile.tileEntity;

        if (targetEntity) {
            targetEntity.TakeDamage(damage);
        }
        currentPlayer.selectedCharacter.currentActionPoints -= cost;
    }
}
