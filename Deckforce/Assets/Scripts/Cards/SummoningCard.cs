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

                    if (newEntity.entityType == Entity.EntityType.TRAP) {
                        targetTile.SetTrap(newEntity.GetComponent<Trap>());
                    } else {
                        targetTile.SetEntity(newEntity);
                    }
                    newEntity.transform.position = new Vector3(
                        targetTile.transform.position.x,
                        targetTile.transform.position.y + 0.5f,
                        targetTile.transform.position.z
                    );

                    if (newEntity.entityType == Entity.EntityType.MONSTER) {
                        currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);
                        newEntity.GetComponent<Pathfinding>().startTile = targetTile;
                        newEntity.GetComponent<AIMonster>().playerOwner = currentPlayer;
                    }
                    ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                    ActivateParticle(targetParticle, targetTile.tileEntity.transform.position,
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
