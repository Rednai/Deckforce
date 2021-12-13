using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Basic Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public int id;
    public string description;
    public Color color;
    
    public Sprite visual;

    public int cost;
    public RangeType areaTypePattern;
    public RangeType effectTypePattern;
    public int playerRange;
    public int effectRange;
    public bool targetEntity;
    public bool areablockByEntity;
    public bool effectblockByEntity;

    public AudioClip activateClip;
    public AudioClip cannotClip;

    public ParticleManager userParticle;
    public ParticleManager targetParticle;

    public bool isActivated = false;

    public List<Effect> effects;

    public virtual void Select()
    {}

    public virtual bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        return (false);
    }

    protected void ActivateParticle(ParticleManager particle, Vector3 initialPosition, Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (particle != null) {
            ParticleManager targetPM = Instantiate(particle, initialPosition, Quaternion.identity);
            targetPM.sourcePosition = sourcePosition;
            targetPM.targetPosition = targetPosition;
        }
    }

    protected virtual bool CheckIfPossible(Player currentPlayer, List<Tile> selectedTiles = null)
    {
        if (currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            return (true);
        }
        return (false);
    }

    protected bool CheckIfAlly(Player currentPlayer, Entity targetEntity)
    {
        if (targetEntity == currentPlayer.selectedCharacter) {
            return (true);
        }
        return (currentPlayer.selectedCharacter.alliedEntities.Contains(targetEntity));
    }

    protected virtual void ActivateEffects(Effect.TargetType targetType, Entity targetEntity)
    {
        foreach (Effect effect in effects) {
            if (effect.targetType == targetType) {
                targetEntity.AddEffect(effect);
            }
        }
    }
}
