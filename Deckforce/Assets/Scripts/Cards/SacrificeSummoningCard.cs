using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Sacrifice Summoning Card")]
public class SacrificeSummoningCard : SummoningCard
{
    public Entity.EntityType sacrificeType;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer, targetsTiles)) {

            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            foreach (Tile targetTile in targetsTiles) {
                if (targetTile.tileEntity != null &&
                    targetTile.tileEntity.entityType == sacrificeType) {
                    //TODO: retirer l'entité de la liste des alliés du joueur peut etre??
                    Entity oldEntity = targetTile.tileEntity;
                    targetTile.tileEntity = null;
                    Destroy(oldEntity.gameObject);

                    GameObject entityGO = Instantiate(summoningEntity).gameObject;
                    Entity newEntity = entityGO.GetComponent<Entity>();

                    newEntity.GetComponent<Pathfinding>().startTile = targetTile;
                    newEntity.GetComponent<AIMonster>().playerOwner = currentPlayer;
                    targetTile.SetEntity(newEntity);
                    newEntity.transform.position = new Vector3(
                        targetTile.transform.position.x,
                        targetTile.transform.position.y + 0.5f,
                        targetTile.transform.position.z
                    );
                    newEntity.playerId = currentPlayer.id;
                    currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);

                    ActivateParticle(userParticle, currentPlayer.selectedCharacter.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                    ActivateParticle(targetParticle, targetTile.tileEntity.transform.position,
                        currentPlayer.selectedCharacter.transform.position, targetTile.transform.position);
                    isActivated = true;
                    SoundsManager.instance.PlaySound(activateClip);
                    return (true);
                }
            }
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }

    protected override bool CheckIfPossible(Player currentPlayer, List<Tile> selectedTiles)
    {
        if (currentPlayer.selectedCharacter.currentActionPoints < cost) {
            return (false);
        }
        foreach (Tile targetTile in selectedTiles) {
            if (targetTile.tileEntity.entityType == sacrificeType) {
                return (true);
            }
        }
        return (true);
    }
}
