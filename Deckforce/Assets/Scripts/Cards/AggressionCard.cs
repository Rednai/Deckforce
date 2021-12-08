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

        if (targetEntity && CheckIfAlly(currentPlayer, targetEntity) == false &&
            currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            
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
            targetEntity.TakeDamage(damage);
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            return (true);
        }
        return (false);
    }
}
