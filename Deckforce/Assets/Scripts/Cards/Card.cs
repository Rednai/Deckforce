using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public enum RangeType
    {
        CIRCULAR,
        LINEAR,
        SQUARE,
        DIAGONAL,
    }

    public int cost;
    public int playerRange;
    public int effectRange;

    public Sprite visual;
    public string description;
    public string name;
    public Color color;

    public AudioClip activateClip;

    public virtual void Select()
    {}

    public virtual void Activate(Player currentPlayer, Tile targetTile)
    {}
}
