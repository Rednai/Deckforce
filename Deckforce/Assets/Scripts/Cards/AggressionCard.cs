using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Aggression Card")]
public class AggressionCard : Card
{
    public int damage;
    public bool emptyDroppable;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer, targetsTiles)) {
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            isActivated = true;
            ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                currentPlayer.selectedCharacter.transform.position, centerTile.transform.position);
            
            //TODO: faire en sorte d'activer les target particles quand le projectile arrive à sa case
            foreach (Tile targetTile in targetsTiles) {
                Entity targetEntity = targetTile.tileEntity;

                ActivateParticle(targetParticle, targetTile.tileEntity.transform.position, currentPlayer.selectedCharacter.transform.position,
                    targetTile.transform.position);

                if (targetEntity && !CheckIfAlly(currentPlayer, targetEntity)) {
                    targetEntity.TakeDamage(damage);
                    ActivateEffects(Effect.TargetType.TARGET, targetEntity);
                }
            }
            ActivateEffects(Effect.TargetType.SELF, currentPlayer.selectedCharacter);
            SoundsManager.instance.PlaySound(activateClip);
            return (true);
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
            if (tile.tileEntity == null && !emptyDroppable) {
                return (false);
            }
        }
        return (true);
    }
}
