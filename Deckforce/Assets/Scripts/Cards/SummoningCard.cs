using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Summoning Card")]
public class SummoningCard : Card
{
    public Entity summoningEntity;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer, targetsTiles)) {
            
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            
            foreach (Tile targetTile in targetsTiles) {
                if (CheckIfTileEmpty(targetTile)) {
                    GameObject entityGO = Instantiate(summoningEntity).gameObject;
                    Entity newEntity = entityGO.GetComponent<Entity>();

                    switch (newEntity.entityType) {
                        case (Entity.EntityType.TRAP):
                            targetTile.SetTrap(newEntity.GetComponent<Trap>());
                            break;
                        case (Entity.EntityType.MONSTER):
                        case (Entity.EntityType.PROP):
                            targetTile.SetEntity(newEntity);
                            break;
                    }
                    newEntity.transform.position = new Vector3(
                        targetTile.transform.position.x,
                        targetTile.transform.position.y + 0.5f,
                        targetTile.transform.position.z
                    );

                    if (newEntity.entityType == Entity.EntityType.MONSTER) {
                        currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);
                        newEntity.playerId = currentPlayer.id;
                        newEntity.GetComponent<Pathfinding>().startTile = targetTile;
                        if (newEntity.GetComponent<AIMonster>() != null)
                            newEntity.GetComponent<AIMonster>().playerOwner = currentPlayer;
                            //TODO: mettre playerowner qqpart accessible par les 2 classes
                        if (newEntity.GetComponent<Bomb>() != null)
                            newEntity.GetComponent<Bomb>().playerOwner = currentPlayer;
                    }
                    switch (newEntity.entityType) {
                        case (Entity.EntityType.TRAP):
                            ActivateParticle(targetParticle, targetTile.tileTrap.transform.position,
                                currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                            break;
                        case (Entity.EntityType.MONSTER):
                        case (Entity.EntityType.PROP):
                            ActivateParticle(targetParticle, targetTile.tileEntity.transform.position,
                                currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                            break;
                    }
                    ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                    isActivated = true;
                    SoundsManager.instance.PlaySound(activateClip);
                }
                return (true);
            }
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }

    protected override bool CheckIfPossible(Player currentPlayer, List<Tile> selectedTiles)
    {
        if (!base.CheckIfPossible(currentPlayer, selectedTiles)) {
            return (false);
        }
        foreach (Tile selectedTile in selectedTiles) {
            if (selectedTile.tileEntity == null && selectedTile.tileTrap == null) {
                return (true);
            }
        }
        return (false);
    }

    bool CheckIfTileEmpty(Tile selectedTile)
    {
        if (selectedTile.tileEntity == null && selectedTile.tileTrap == null) {
            return (true);
        }
        return (false);
    }
}
