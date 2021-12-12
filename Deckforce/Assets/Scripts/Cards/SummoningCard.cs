using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Summoning Card")]
public class SummoningCard : Card
{
    public Entity summoningEntity;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        foreach (Tile targetTile in targetsTiles) {
            if (targetTile.tileEntity == null && 
                currentPlayer.selectedCharacter.currentActionPoints >= cost) {
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

                currentPlayer.selectedCharacter.currentActionPoints -= cost;
                if (newEntity.entityType == Entity.EntityType.MONSTER) {
                    currentPlayer.selectedCharacter.alliedEntities.Add(newEntity);
                    newEntity.GetComponent<Pathfinding>().startTile = targetTile;
                    newEntity.GetComponent<AIMonster>().playerOwner = currentPlayer;
                }
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
                isActivated = true;
                return (true);
            }
        }
        return (false);
    }
}
