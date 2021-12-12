using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Aggression Card")]
public class AggressionCard : Card
{
    public int damage;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            isActivated = true;
            if (userParticle != null) {
                ParticleManager userPM = Instantiate(userParticle, currentPlayer.selectedCharacter.transform.position, Quaternion.identity);
                userPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                userPM.targetPosition = centerTile.transform.position;
            }
            //TODO: faire en sorte d'activer les target particles quand le projectile arrive à sa case
            foreach (Tile targetTile in targetsTiles) {
                Entity targetEntity = targetTile.tileEntity;

                if (targetParticle != null) {
                    ParticleManager targetPM = Instantiate(targetParticle, targetTile.tileEntity.transform.position, Quaternion.identity);
                    targetPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                    targetPM.targetPosition = targetTile.transform.position;
                }

                if (targetEntity && !CheckIfAlly(currentPlayer, targetEntity)) {
                    targetEntity.TakeDamage(damage);
                    ActivateEffects(Effect.TargetType.TARGET, targetEntity);
                }
            }
            ActivateEffects(Effect.TargetType.SELF, currentPlayer.selectedCharacter);
            return (true);
        }
        return (false);
    }
}
