using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cost;
    public int range;

    public AudioClip activateClip;

    public virtual void Activate()
    {}
}
