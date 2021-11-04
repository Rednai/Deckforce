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
        entityGO.transform.position = targetTile.centerPosition.transform.position;
        Entity newEntity = entityGO.GetComponent<Entity>();

        targetTile.tileEntity = newEntity;

        currentPlayer.selectedCharacter.currentActionPoints -= cost;
    }
}
