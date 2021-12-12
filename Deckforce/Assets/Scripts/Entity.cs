using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType {
        CHARACTER,
        MONSTER,
        PROP,
        TRAP
    };
    public EntityType entityType;
    public string entityName;
    public int id;
    public int currentLife;
    public int maxLife;
    public int currentShield;
    public int maxShield;
    public int currentMovePoints;
    public int maxMovePoints;
    public Sprite entityIcon;

    public int initiative;

    Animator animator;
    AudioSource audioSource;

    public bool canMove = false;

    public List<Effect> appliedEffects;

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
    {
        currentMovePoints = maxMovePoints;
        canMove = true;
        foreach (Effect effect in appliedEffects) {
            if (effect.activationTime == Effect.ActivationTime.STARTTURN) {
                effect.Activate(this);
            }
        }
    }

    public virtual void EndTurn()
    {
        foreach (Effect effect in appliedEffects) {
            if (effect.activationTime == Effect.ActivationTime.ENDTURN) {
                effect.Activate(this);
            }
        }
    }

    public virtual void Move(Tile targetTile)
    {}

    public virtual void AddShield(int shieldAmount)
    {
        currentShield += shieldAmount;
        if (currentShield > maxShield) {
            currentShield = maxShield;
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (currentShield >= damageAmount) {
            currentShield -= damageAmount;
        } else {
            damageAmount -= currentShield;
            currentShield = 0;
        }
        currentLife -= damageAmount;

        if (currentLife <= 0) {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void AddEffect(Effect newEffect)
    {
        bool isAdded = false;
        foreach (Effect effect in appliedEffects) {
            if (effect.GetType() == newEffect.GetType()) {
                effect.remainingTurns++;
                isAdded = true;
            }
        }
        if (isAdded == false) {
            appliedEffects.Add(Instantiate(newEffect));
        }
    }
}
