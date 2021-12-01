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

            targetTile.tileEntity = newEntity;
            newEntity.transform.position = new Vector3(
                targetTile.transform.position.x,
                targetTile.transform.position.y + 0.5f,
                targetTile.transform.position.z
            );

            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);

            if (userParticle != null) {
                Instantiate(userParticle, currentPlayer.selectedCharacter.transform.position, Quaternion.identity);
            }
            if (targetParticle != null) {
                Instantiate(targetParticle, targetEntity.transform.position, Quaternion.identity);
            }
            return (true);
        }
        return (false);
    }
}
