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
                ParticleManager userPM = Instantiate(userParticle, currentPlayer.selectedCharacter.transform.position, Quaternion.identity);
                userPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                userPM.targetPosition = targetTile.transform.position;
            }
            if (targetParticle != null) {
                ParticleManager targetPM = Instantiate(targetParticle, targetTile.tileEntity.transform.position, Quaternion.identity);
                targetPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                targetPM.targetPosition = targetTile.transform.position;
            }
            isActivated = true;
            return (true);
        }
        return (false);
    }
}
