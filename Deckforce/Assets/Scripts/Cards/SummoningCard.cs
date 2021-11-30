using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Summoning Card")]
public class SummoningCard : Card
{
    public Entity summoningEntity;

    public override bool Activate(Player currentPlayer, Tile targetTile)
    {
        if (targetTile.tileEntity == null && 
            currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            GameObject entityGO = Instantiate(summoningEntity).gameObject;
            Entity newEntity = entityGO.GetComponent<Entity>();

            targetTile.tileEntity = newEntity;
            newEntity.transform.position = new Vector3(
                targetTile.transform.position.x,
                targetTile.transform.position.y + 0.5f,
                targetTile.transform.position.z
            );

            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            if (newEntity.entityType == Entity.EntityType.MONSTER) {
                currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);
            }
            return (true);
        }
        return (false);
    }
}
