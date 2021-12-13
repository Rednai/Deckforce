using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card/Heal Card")]
public class HealCard : ManipulationCard
{
    public int healAmount;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer)) {

            currentPlayer.selectedCharacter.currentActionPoints -= cost;

            foreach (Tile targetTile in targetsTiles) {
                Entity targetEntity = targetTile.tileEntity;

                if (targetEntity && CheckIfAlly(currentPlayer, targetTile.tileEntity)) {
                    targetTile.tileEntity.Heal(healAmount);

                    ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                    ActivateParticle(targetParticle, targetTile.tileEntity.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);

                    isActivated = true;
                    SoundsManager.instance.PlaySound(activateClip);
                    return (true);
                }
            }
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }
}
