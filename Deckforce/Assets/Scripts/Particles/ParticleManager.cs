using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public Vector3 sourcePosition;
    public Vector3 targetPosition;
    public float particleDuration;

    void Update() {
        particleDuration -= Time.deltaTime;
        if (particleDuration <= 0) {
            Destroy(gameObject);
        }
    }
}
