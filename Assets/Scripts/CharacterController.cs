using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public ActiveRagdoll walker;
    public WalkerNodeFollower walkerNodeFollower;
    public DogController dog;

    [Space]

    public float minDistance = 0.4f;
    public float maxDistance = 3.0f;
    public float maxPullDistance = 4.0f;
    public AnimationCurve distanceSpeedModifierCurve;

    [Space]

    public float maxStamina = 100.0f;
    public float staminaRegenRate = 10.0f;
    public float staminaDrainRate = 25.0f;

    private bool faceDog = false;
    private bool faceDogPrev = false;
    private ActiveRagdoll.BalanceState currentState = ActiveRagdoll.BalanceState.Balanced;

    private void UpdateState() {

        if (DogOutsideRange()) {
            currentState = ActiveRagdoll.BalanceState.Unbalanced;
            faceDog = true;
            walker.SetBalanceState(currentState);
        } 
        else {
            currentState = ActiveRagdoll.BalanceState.Balanced;
            faceDog = false;
            walker.SetBalanceState(currentState);
        }
    }

    private bool DogOutsideRange() {
        return Vector3.Distance(GetWalkerTransform().position, GetDogTransform().position) > maxDistance;
    }

    // Evaluates the distance of the position between the max distance and the max pull distance and
    // returns a modifier to slow the dog based on the distance speed modifier curve.
    public float GetMaxDistanceSpeedModifier(Vector3 position) {
        float distance = Vector3.Distance(GetWalkerTransform().position, position);
        float percent = Mathf.Clamp01((distance - maxDistance) / (maxPullDistance - maxDistance));
        return distanceSpeedModifierCurve.Evaluate(percent);
    }

    public float GetMinDistanceSpeedModifier(Vector3 position) {
        float distance = Vector3.Distance(GetWalkerTransform().position, position);
        
        if (distance < minDistance)
            return 0.0f;
        return 1.0f;
    }

    public Transform GetWalkerTransform() {
        return walker.transform;
    }
    
    public Transform GetDogTransform() {
        return dog.transform;
    }

    private void Update() {
        UpdateState();

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (faceDog) {
            walkerNodeFollower.agent.destination = GetDogTransform().position;
        } else if (faceDogPrev && input.magnitude > 0.0f) {
            walkerNodeFollower.ReselectCurrentNode();
        }

        faceDogPrev = faceDog;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetWalkerTransform().position, maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetWalkerTransform().position, maxPullDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetWalkerTransform().position, minDistance);
    }
}
