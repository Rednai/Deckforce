using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastToTarget : MonoBehaviour {
    public float particleSpeed;
    public Transform particleObject;
    ParticleManager particleManager;
    float distance;


    void Start() {
        particleManager = GetComponent<ParticleManager>();
        float time = distance / particleSpeed;
        distance = Vector2.Distance(
            new Vector2(particleManager.sourcePosition.x, particleManager.sourcePosition.z),
            new Vector2(particleManager.targetPosition.x, particleManager.targetPosition.z)
        );
        particleManager.particleDuration = time + 3;
        Debug.Log($"SourcePosition: {particleManager.sourcePosition}, TargetPosition: {particleManager.targetPosition}, Distance: {distance}, Time: {time}");
        transform.LookAt(new Vector3(particleManager.targetPosition.x, particleManager.sourcePosition.y, particleManager.targetPosition.z));
    }

    void Update() {
        if (particleObject.localPosition.z < distance)
            particleObject.localPosition = particleObject.localPosition + (Vector3.forward * particleSpeed * Time.deltaTime);
        else {
            // TODO: Trigger death
        }
    }
}
