using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCard : Card
{
    public Entity summoningEntity;

    public override void Activate(Tile targetTile)
    {
        GameObject entityGO = Instantiate(summoningEntity).gameObject;
        entityGO.transform.position = targetTile.centerPosition.transform.position;
        Entity newEntity = entityGO.GetComponent<Entity>();

        targetTile.tileEntity = newEntity;
    }
}
