using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerNodeFollower : NodeFollower
{
    public CharacterController characterController;

    public float maxSpeed;
    public AnimationCurve slowCurve;
    
    private Rigidbody localRigidbody;

    protected void Awake() {
        localRigidbody = GetComponent<Rigidbody>();
    }

    // If the dog is behind us and we're moving, we should slow down.
    protected float GetMovementModifier() {
        float distance = Vector3.Distance(transform.position, characterController.GetDogTransform().position);

        // Debug.Log(dot);
        
        Debug.DrawLine(localRigidbody.position, localRigidbody.position + localRigidbody.velocity, Color.red);
        Debug.DrawRay(localRigidbody.position, characterController.GetDogTransform().position - localRigidbody.position, Color.green);

        float percent = Mathf.Clamp01((distance - characterController.maxDistance) / (characterController.maxPullDistance - characterController.maxDistance));

        return 1.0f;  // Temp
    }

    protected override void Update() {
        base.Update();

        agent.speed = maxSpeed * GetMovementModifier();
    }
}
