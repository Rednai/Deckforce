using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Aggression Card")]
public class AggressionCard : Card
{
    public enum ExplosionType { SINGLETILE, SQUARE, ARC };
    public ExplosionType explosionType;
    public int damage;

    public override bool Activate(Player currentPlayer, Tile targetTile)
    {
        Entity targetEntity = targetTile.tileEntity;

        if (targetEntity && CheckIfAlly(currentPlayer, targetEntity) == false && currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            targetEntity.TakeDamage(damage);
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            return (true);
        }
        return (false);
    }

    bool CheckIfAlly(Player currentPlayer, Entity targetEntity)
    {
        if (targetEntity == currentPlayer.selectedCharacter) {
            return (true);
        }
        return (currentPlayer.selectedCharacter.alliedEntities.Contains(targetEntity));
    }
}
