using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int currentLife;
    public int maxLife;

    Animator animator;
    AudioSource audioSource;

    public virtual void Move(Tile targetTile)
    {}

    public virtual void TakeDamge(int damageAmount)
    {}

    public virtual void Die()
    {}
}
