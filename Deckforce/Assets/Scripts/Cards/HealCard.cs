using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card/Heal Card")]
public class HealCard : ManipulationCard
{
    public int healAmount;
    public bool droppableOnNothing;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer, targetsTiles)) {

            currentPlayer.selectedCharacter.currentActionPoints -= cost;

            foreach (Tile targetTile in targetsTiles) {
                Entity targetEntity = targetTile.tileEntity;

                ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);

                if (targetEntity && CheckIfAlly(currentPlayer, targetTile.tileEntity)) {
                    targetTile.tileEntity.Heal(healAmount);



                    if (targetEntity != null)
                        ActivateParticle(targetParticle, targetTile.tileEntity.transform.position, currentPlayer.selectedCharacter.transform.position,
                        targetTile.transform.position);
                    else
                        ActivateParticle(targetParticle, targetTile.transform.position, currentPlayer.selectedCharacter.transform.position,
                        targetTile.transform.position);

                    isActivated = true;
                    SoundsManager.instance.PlaySound(activateClip);
                    return (true);
                }
            }
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }

    protected override bool CheckIfPossible(Player currentPlayer, List<Tile> selectedTiles = null)
    {
        if (!base.CheckIfPossible(currentPlayer, selectedTiles)) {
            return (false);            
        }
        foreach (Tile tile in selectedTiles) {
            if (tile.tileEntity == null && !droppableOnNothing) {
                return (false);
            } else if (tile.tileEntity != null && CheckIfAlly(currentPlayer, tile.tileEntity)) {
                return (true);
            }
        }
        return (true);
    }
}
