using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Entity
{
    public int damage;

    public void Activate(Entity entity)
    {
        entity.TakeDamage(damage);
        Destroy(gameObject);
    }
}
