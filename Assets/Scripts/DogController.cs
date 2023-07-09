using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public CharacterController characterController;
    public Animator localAnimator;

    [Header("Movement")]

    public float maxSpeed;
    public AnimationCurve accelerationCurve;
    public float accelerationTime;
    public AnimationCurve decelerationCurve;
    public float decelerationTime;
    public float groundOffset;
    public float maxSlopeAngle = 60.0f;

    [Header("Collisions")]

    public float collisionRadius;
    public LayerMask collisionMask;


    private Rigidbody localRigidbody;
    private Vector3 velocity;
    private Vector2 input;
    private float timer;

    [HideInInspector] public bool isFrenzied = false;

    private void Awake() {
        localRigidbody = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        input = Vector2.zero;
        timer = 0f;
    }

    private void UpdateInputs() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void UpdateVelocity() {
        Vector3 targetVelocity = new Vector3(input.x, 0f, input.y) * maxSpeed;

        // Timer goes up when we're accelerating, down when we're decelerating
        if (input.magnitude == 0f) {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0f, Mathf.Max(accelerationTime, decelerationTime));
            Decelerate();
        } else {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0f, Mathf.Max(accelerationTime, decelerationTime));
            Accelerate();
        }
    }

    private void StickToGround() {
        // Get the height of the ground at the current location.
        RaycastHit hit = GetGroundHit(localRigidbody.position);
        
        float groundHeight = 0.0f;
        if (hit.collider != null) {
            groundHeight = hit.point.y;
        }

        // If the dog is below the ground, move it up.
        if (localRigidbody.position.y < groundHeight + groundOffset) {
            localRigidbody.position = new Vector3(localRigidbody.position.x, groundHeight + groundOffset, localRigidbody.position.z);
        }
    }

    private void HandleSlopes() {
        RaycastHit hit = GetGroundHit(localRigidbody.position + (velocity * Time.deltaTime));

        // Check the angle of the ground - if it's too steep we should simply stop moving.
        if (Vector3.Angle(hit.normal, Vector3.up) > maxSlopeAngle) {
            velocity = Vector3.zero;
        }
    }

    private RaycastHit GetGroundHit(Vector3 position) {
        RaycastHit hit;
        Physics.Raycast(position + (Vector3.up * 1000f), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider == null) {
            Debug.LogWarning("Unable to find ground.");
        }
        return hit;
    }

    private void RotateTowardsInput() {
        if (input.magnitude == 0f)
            return;

        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + 180f;
        float lerpAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, 0.25f);
        localRigidbody.rotation = Quaternion.Euler(0f, lerpAngle, 0f);
    }

    private void ApplyLeashModifier() {
        Vector3 inputNormalized = new Vector3(input.normalized.x, 0f, input.normalized.y);
        float dot = Vector3.Dot(inputNormalized, (characterController.GetWalkerTransform().position - localRigidbody.position).normalized);

        if (dot < 0.0f)
            velocity *= characterController.GetMaxDistanceSpeedModifier(localRigidbody.position);
    }

    private void ApplyMinDistanceModifier() {
        Vector3 inputNormalized = new Vector3(input.normalized.x, 0f, input.normalized.y);
        float dot = -1.0f * Vector3.Dot(inputNormalized, (characterController.GetWalkerTransform().position - localRigidbody.position).normalized);

        if (dot < 0.0f)
            velocity *= characterController.GetMinDistanceSpeedModifier(localRigidbody.position);
    }

    private void Accelerate() {
        float acceleration = accelerationCurve.Evaluate(Mathf.Clamp01(timer / accelerationTime));
        float speed = Mathf.Clamp(acceleration * maxSpeed, 0.0f, maxSpeed);
        velocity = new Vector3(input.x, 0f, input.y).normalized * speed;
    }

    private void Decelerate() {
        float deceleration = decelerationCurve.Evaluate(Mathf.Clamp01(timer / decelerationTime));
        float speed = Mathf.Clamp(deceleration * maxSpeed, 0.0f, maxSpeed);
        velocity = new Vector3(input.x, 0f, input.y).normalized * speed;
    }

    private float GetLinearVelocity() {
        return new Vector3(velocity.x, 0f, velocity.z).magnitude;
    }

    private void UpdateAnimator() {
        localAnimator.SetFloat("Speed", Mathf.Clamp01(GetLinearVelocity() / maxSpeed));
        localAnimator.SetBool("Frenzy", characterController.isFrenzied);
    }

    private void CheckCollisions() {
        RaycastHit hit;
        Physics.Raycast(localRigidbody.position, velocity.normalized, out hit, collisionRadius, collisionMask);
        if (hit.collider != null) {
            velocity = Vector3.zero;
            // localRigidbody.position += (hit.point - localRigidbody.position).normalized * (hit.distance - collisionRadius);
        }
    }

    private void Update() {
        UpdateInputs();
        StickToGround();
        RotateTowardsInput();
        UpdateAnimator();
    }

    private void FixedUpdate() {
        UpdateVelocity();
        if (!characterController.isFrenzied)
            ApplyLeashModifier();
        ApplyMinDistanceModifier();
        CheckCollisions();
        HandleSlopes();

        velocity.y = Physics.gravity.y;
        localRigidbody.position += velocity * Time.deltaTime;
    }
}
