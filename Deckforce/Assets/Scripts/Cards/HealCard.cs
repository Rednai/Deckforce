using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card/Heal Card")]
public class HealCard : ManipulationCard
{
    public int healAmount;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        foreach (Tile targetTile in targetsTiles)
        {
            Entity targetEntity = targetTile.tileEntity;

            if (targetEntity && CheckIfAlly(currentPlayer, targetTile.tileEntity) &&
                currentPlayer.selectedCharacter.currentActionPoints >= cost)
            {
                targetTile.tileEntity.Heal(healAmount);
                currentPlayer.selectedCharacter.currentActionPoints -= cost;

                if (userParticle != null)
                {
                    ParticleManager userPM = Instantiate(userParticle, currentPlayer.selectedCharacter.transform.position, Quaternion.identity);
                    userPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                    userPM.targetPosition = targetTile.transform.position;
                }
                if (targetParticle != null)
                {
                    ParticleManager targetPM = Instantiate(targetParticle, targetTile.tileEntity.transform.position, Quaternion.identity);
                    targetPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                    targetPM.targetPosition = targetTile.transform.position;
                }
                isActivated = true;
                SoundsManager.instance.PlaySound(activateClip);
                return (true);
            }
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }
}
