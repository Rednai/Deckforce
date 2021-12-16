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
    public int battleId;
    public string playerId;
    public int currentLife;
    public int maxLife;
    public int currentShield;
    public int maxShield;
    public int currentMovePoints;
    public int maxMovePoints;
    public Sprite entityIcon;
    public AudioClip spawnSound;
    public AudioClip hurtSound;
    public AudioClip shieldHit;

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
        SoundsManager.instance.PlaySound(spawnSound);
        currentLife = maxLife;
        currentMovePoints = maxMovePoints;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void StartTurn()
    {
        currentMovePoints = maxMovePoints;
        canMove = true;
        ApplyEffects(Effect.ActivationTime.STARTTURN);
    }

    public virtual void EndTurn()
    {
        ApplyEffects(Effect.ActivationTime.ENDTURN);
    }

    protected void ApplyEffects(Effect.ActivationTime activationTime)
    {
        foreach (Effect effect in appliedEffects) {
            if (effect.activationTime == activationTime) {
                effect.Activate(this);
            }
        }
        ClearEffects();
    }

    void ClearEffects()
    {
        List<Effect> effectsToRemove = appliedEffects.FindAll(x => x.remainingTurns == 0);

        if (effectsToRemove.Count != 0) {
            foreach (Effect effect in effectsToRemove) {
                appliedEffects.Remove(effect);
            }
        }
    }

    public virtual void Move(Tile targetTile)
    {}

    public virtual void AddShield(int shieldAmount)
    {
        Debug.Log("Add shield to entity");
        currentShield += shieldAmount;
        if (currentShield > maxShield) {
            currentShield = maxShield;
        }
    }

    public virtual void Heal(int healAmount)
    {
        currentLife += healAmount;
        if (currentLife > maxLife)
        {
            currentLife = maxLife;
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (currentShield >= damageAmount) {
            currentShield -= damageAmount;
            SoundsManager.instance.PlaySound(shieldHit);
            return ;
        } else {
            damageAmount -= currentShield;
            currentShield = 0;
        }
        SoundsManager.instance.PlaySound(hurtSound);
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
