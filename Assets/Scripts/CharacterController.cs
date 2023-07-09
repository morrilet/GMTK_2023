using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public ActiveRagdoll walker;
    public WalkerNodeFollower walkerNodeFollower;
    public DogController dog;

    [Space]

    public Animator walkerAnimator;

    [Space]

    public float minDistance = 0.4f;
    public float maxDistance = 3.0f;
    public float maxPullDistance = 4.0f;
    public AnimationCurve distanceSpeedModifierCurve;

    [Space]

    public float maxStamina = 100.0f;
    public float staminaRegenRate = 10.0f;
    public float staminaDrainRate = 25.0f;

    [Space]

    public float getUpTime = 3.0f;

    private bool faceDog = false;
    private bool faceDogPrev = false;
    private float stamina = 0.0f;
    private float getUpTimer = 0.0f;
    private ActiveRagdoll.BalanceState currentState = ActiveRagdoll.BalanceState.Balanced;

    // TEMP TESTING
    public SkinnedMeshRenderer anim_mesh;
    public SkinnedMeshRenderer ragdoll_mesh;
    public bool toggle = false;

    [HideInInspector] public bool isFrenzied = false;

    private void Awake() {
        GameManager.instance.AssignCharacterController(this);
        stamina = maxStamina;
    }

    private void UpdateState() {

        if (currentState == ActiveRagdoll.BalanceState.Falling) {
            if (getUpTimer <= getUpTime) {
                walkerNodeFollower.agent.enabled = false;
                walkerNodeFollower.enabled = false;
                return;
            }
            walkerNodeFollower.agent.enabled = true;
            walkerNodeFollower.enabled = true;
        }

        if (DogAtMaxRange()) {
            currentState = ActiveRagdoll.BalanceState.Unbalanced;
            faceDog = stamina > 0.0f;
            walker.SetBalanceState(currentState);
        }
        else if (DogAtComfortableRange() || stamina <= 0.0f) {
            currentState = ActiveRagdoll.BalanceState.Balanced;
            faceDog = false;
            walker.SetBalanceState(currentState);
        }

        // TEMP TESTING
        // if (Input.GetKeyDown(KeyCode.T)) {
        //     anim_mesh.enabled = toggle;
        //     ragdoll_mesh.enabled = !toggle;
        //     toggle = !toggle;
        // }
    }

    public void TriggerFrenzy() {
        if (isFrenzied) {
            return;
        }

        currentState = ActiveRagdoll.BalanceState.Falling;
        faceDog = false;
        isFrenzied = true;
        walker.SetBalanceState(currentState);
    }

    private void UpdateWalkerAnimator() {
        walkerAnimator.SetFloat("Speed", Mathf.Clamp01(walkerNodeFollower.agent.velocity.magnitude / walkerNodeFollower.maxSpeed));
    }

    private void UpdateStamina() {
        if (currentState == ActiveRagdoll.BalanceState.Unbalanced) {
            stamina = Mathf.Clamp(stamina - staminaDrainRate * Time.deltaTime, 0.0f, maxStamina);
        } else if (currentState == ActiveRagdoll.BalanceState.Balanced) {
            stamina = Mathf.Clamp(stamina + staminaRegenRate * Time.deltaTime, 0.0f, maxStamina);
        }
    }

    private void UpdateGetUpTimer() {
        if (currentState == ActiveRagdoll.BalanceState.Falling) {
            getUpTimer += Time.deltaTime;
        } else {
            getUpTimer = 0.0f;
        }
    }

    private void UpdateFrenzied() {
        if (currentState == ActiveRagdoll.BalanceState.Falling) {
            isFrenzied = true;
        } else {
            isFrenzied = false;
        }
    }

    private void ForceWalkerToDogRadius() {
        Vector3 direction = GetDogTransform().position - GetWalkerTransform().position;
        direction.y = 0.0f;

        if (direction.magnitude > maxPullDistance) {
            walker.AddForce(direction.normalized * 10f);
        }
    }

    private bool DogAtMaxRange(float tolerance = 0.5f) {
        // Include a tolerance just so we don't have to be exactly at the max distance to be considered at max range.
        return Vector3.Distance(GetWalkerTransform().position, GetDogTransform().position) >= maxPullDistance - tolerance;
    }

    private bool DogAtComfortableRange() {
        return Vector3.Distance(GetWalkerTransform().position, GetDogTransform().position) <= maxDistance;
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
        UpdateStamina();
        UpdateGetUpTimer();
        UpdateFrenzied();

        if (isFrenzied)
            ForceWalkerToDogRadius();

        UpdateWalkerAnimator();

        Vector3 input = new Vector3(Input.GetAxis(GlobalVariables.INPUT_HORIZONTAL), 0.0f, Input.GetAxis(GlobalVariables.INPUT_VERTICAL));

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
