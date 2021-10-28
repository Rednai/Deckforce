using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public int cost;
    public int range;

    public Sprite visual;
    public string description;

    public AudioClip activateClip;

    public virtual void Activate(Tile targetTile)
    {}
}
