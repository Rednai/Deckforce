using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Shield Card")]
public class ShieldCard : ManipulationCard
{
    public int shieldAmount;

    public override bool Activate(Player currentPlayer, Tile targetTile)
    {
        if (CheckIfAlly(currentPlayer, targetTile.tileEntity) && 
            currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            Debug.Log("ally");
            targetTile.tileEntity.AddShield(shieldAmount);
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            
            if (userParticle != null) {
                Instantiate(userParticle, currentPlayer.selectedCharacter.transform.position, Quaternion.identity);
            }
            if (targetParticle != null) {
                Instantiate(targetParticle, targetTile.tileEntity.transform.position, Quaternion.identity);
            }
            return (true);
        }
        return (false);
    }
}
