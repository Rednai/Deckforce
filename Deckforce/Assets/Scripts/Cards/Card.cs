using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public int cost;
    public enum ActivationRange { CROSS, SQUARE, DIAGONALS };
    public ActivationRange activationRange;
    public int range;

    public Sprite visual;
    public Color cardColor;
    public string description;

    public AudioClip activateClip;

    public ParticleSystem userParticle;
    public ParticleSystem targetParticle;

    public Player playerOwner;

    public virtual void Select()
    {}

    public virtual void Activate(Player currentPlayer, Tile targetTile)
    {}
}
