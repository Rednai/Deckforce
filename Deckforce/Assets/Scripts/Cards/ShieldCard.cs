using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Shield Card")]
public class ShieldCard : ManipulationCard
{
    public int shieldAmount;

    public override bool Activate(Player currentPlayer, Tile targetTile)
    {
        Debug.Log(targetTile.tileEntity.entityName + ", " + currentPlayer.selectedCharacter.entityName);
        if (CheckIfAlly(currentPlayer, targetTile.tileEntity) && 
            currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            Debug.Log("ally");
            targetTile.tileEntity.AddShield(shieldAmount);
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            return (true);
        }
        return (false);
    }
}
