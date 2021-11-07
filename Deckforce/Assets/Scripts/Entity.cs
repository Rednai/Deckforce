using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;
    public int currentLife;
    public int maxLife;
    public Sprite entityIcon;

    public int initiative;

    Animator animator;
    AudioSource audioSource;

    public bool canMove = false;

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        currentLife = maxLife;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void StartTurn()
    {}

    public virtual void EndTurn()
    {}

    public virtual void Move(Tile targetTile)
    {}

    public virtual void TakeDamage(int damageAmount)
    {
        currentLife -= damageAmount;

        if (currentLife <= 0) {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log("DIE");
        Destroy(gameObject);
    }
}
