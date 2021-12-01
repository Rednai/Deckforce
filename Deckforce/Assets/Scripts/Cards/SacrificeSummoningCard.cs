using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Sacrifice Summoning Card")]
public class SacrificeSummoningCard : SummoningCard
{
    public Entity.EntityType sacrificeType;

    public override bool Activate(Player currentPlayer, Tile targetTile)
    {
        if (targetTile.tileEntity != null &&
        currentPlayer.selectedCharacter.currentActionPoints >= cost &&
        targetTile.tileEntity.entityType == sacrificeType) {
            Entity oldEntity = targetTile.tileEntity;
            targetTile.tileEntity = null;
            Destroy(oldEntity.gameObject);

            GameObject entityGO = Instantiate(summoningEntity).gameObject;
            Entity newEntity = entityGO.GetComponent<Entity>();

            targetTile.SetEntity(newEntity);
            newEntity.transform.position = new Vector3(
                targetTile.transform.position.x,
                targetTile.transform.position.y + 0.5f,
                targetTile.transform.position.z
            );

            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);

            if (userParticle != null) {
                ParticleManager userPM = Instantiate(userParticle, currentPlayer.selectedCharacter.transform.position, Quaternion.identity);
                userPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                userPM.targetPosition = targetTile.transform.position;
            }
            if (targetParticle != null) {
                ParticleManager targetPM = Instantiate(targetParticle, targetTile.transform.position, Quaternion.identity);
                targetPM.sourcePosition = currentPlayer.selectedCharacter.transform.position;
                targetPM.targetPosition = targetTile.transform.position;
            }
            return (true);
        }
        return (false);
    }
}
