using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Basic Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Color color;
    
    public Sprite visual;

    public enum AreaTypePattern {
        CIRCULAR, LINEAR, SQUARE, DIAGONAL
    };
    /*
    public enum EffectPattern {
        CROSS, SQUARE, DIAGONALS
    };
    */

    public int cost;
    public AreaTypePattern areaTypePattern;
    public int playerRange;
    public int effectRange;

    public AudioClip activateClip;

    public ParticleSystem userParticle;
    public ParticleSystem targetParticle;

    public virtual void Select()
    {}

    public virtual bool Activate(Player currentPlayer, Tile targetTile)
    {
        return (false);
    }

    protected bool CheckIfAlly(Player currentPlayer, Entity targetEntity)
    {
        if (targetEntity == currentPlayer.selectedCharacter) {
            return (true);
        }
        return (currentPlayer.selectedCharacter.alliedEntities.Contains(targetEntity));
    }
}
