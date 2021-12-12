using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 100;

    public void Activate(Entity entity)
    {
        entity.TakeDamage(damage);
    }
}
