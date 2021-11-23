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

        Debug.Log($"{currentPlayer.playerName} playing character {currentPlayer.selectedCharacter.entityName} targeting {targetEntity.entityName}");
        if (targetEntity && CheckIfAlly(currentPlayer, targetEntity) == false &&
            currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            targetEntity.TakeDamage(damage);
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            return (true);
        }
        return (false);
    }
}
