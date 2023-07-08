using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public ActiveRagdoll walker;
    public DogController dog;

    [Space]

    public float maxDistance = 3.0f;
    public float maxPullDistance = 4.0f;
    public AnimationCurve distanceSpeedModifierCurve;

    // Evaluates the distance of the position between the max distance and the max pull distance and
    // returns a modifier to slow the dog based on the distance speed modifier curve.
    public float GetDistanceSpeedModifier(Vector3 position) {
        float distance = Vector3.Distance(GetWalkerTransform().position, position);
        float percent = Mathf.Clamp01((distance - maxDistance) / (maxPullDistance - maxDistance));
        return distanceSpeedModifierCurve.Evaluate(percent);
    }

    public Transform GetWalkerTransform() {
        return walker.transform;
    }
    
    public Transform GetDogTransform() {
        return dog.transform;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetWalkerTransform().position, maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetWalkerTransform().position, maxPullDistance);
    }
}
