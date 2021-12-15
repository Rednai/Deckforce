using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Shield Card")]
public class ShieldCard : ManipulationCard
{
    public int shieldAmount;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer, targetsTiles)) {

            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            isActivated = true;
            SoundsManager.instance.PlaySound(activateClip);

            foreach (Tile targetTile in targetsTiles) {
                Entity targetEntity = targetTile.tileEntity;

                if (targetEntity && CheckIfAlly(currentPlayer, targetTile.tileEntity)) {
                    targetTile.tileEntity.AddShield(shieldAmount);
                    
                    ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                    ActivateParticle(targetParticle, targetTile.tileEntity.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                }
            }
            return (true);
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }

    protected override bool CheckIfPossible(Player currentPlayer, List<Tile> selectedTiles)
    {
        if (!base.CheckIfPossible(currentPlayer, selectedTiles)) {
            return (false);
        }
        foreach (Tile selectedTile in selectedTiles) {
            if (selectedTile.tileEntity != null && CheckIfAlly(currentPlayer, selectedTile.tileEntity)) {
                return (true);
            }
        }
        return (false);
    }
}
