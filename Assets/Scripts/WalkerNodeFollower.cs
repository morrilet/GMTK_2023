using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerNodeFollower : NodeFollower
{
    public CharacterController characterController;

    public float maxSpeed;
    public AnimationCurve slowCurve;
    
    // private Rigidbody localRigidbody;

    protected void Awake() {
        // localRigidbody = GetComponent<Rigidbody>();
    }

    // If the dog is behind us and we're moving, we should slow down.
    protected float GetMovementModifier() {
        float distance = Vector3.Distance(transform.position, characterController.GetDogTransform().position);
        float dot = Vector3.Dot((characterController.GetDogTransform().position - transform.position).normalized, agent.velocity.normalized);
        float percent = Mathf.Clamp01(distance - characterController.maxDistance);

        if (dot < 0.0f)
            return slowCurve.Evaluate(percent);
        else return 1.0f;
    }

    protected override void Update() {
        base.Update();

        // This does allow the walker to move a little too far away because of 
        // carry-over velocity, but fixing it is too much hassle right now.
        agent.speed = maxSpeed * GetMovementModifier();

        if (agent.velocity.magnitude > agent.speed)
            agent.velocity = agent.velocity.normalized * agent.speed;
    }
}
