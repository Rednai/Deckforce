using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Summoning Card")]
public class SummoningCard : Card
{
    public Entity summoningEntity;

    public override void Activate(Player currentPlayer, Tile targetTile)
    {
        GameObject entityGO = Instantiate(summoningEntity).gameObject;
        // entityGO.transform.position = targetTile.centerPosition.transform.position;
        Entity newEntity = entityGO.GetComponent<Entity>();

        targetTile.tileEntity = newEntity;
        newEntity.transform.position = new Vector3(
            targetTile.transform.position.x,
            targetTile.transform.position.y + 0.5f,
            targetTile.transform.position.z
        );
        //targetTile.transform.position;

        currentPlayer.selectedCharacter.currentActionPoints -= cost;
        currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);
    }
}
