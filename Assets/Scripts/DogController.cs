using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public CharacterController characterController;

    public float maxSpeed;
    public AnimationCurve accelerationCurve;
    public float accelerationTime;
    public AnimationCurve decelerationCurve;
    public float decelerationTime;

    private Rigidbody localRigidbody;
    private Vector3 velocity;
    private Vector2 input;
    private float timer;

    private void Awake() {
        localRigidbody = GetComponent<Rigidbody>();
        localRigidbody.isKinematic = true;
        velocity = Vector3.zero;
        input = Vector2.zero;
        timer = 0f;
    }

    private void Start() {

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
        velocity = new Vector3(input.x, 0f, input.y) * speed;
    }

    private void Decelerate() {
        float deceleration = decelerationCurve.Evaluate(Mathf.Clamp01(timer / decelerationTime));
        float speed = Mathf.Clamp(deceleration * maxSpeed, 0.0f, maxSpeed);
        velocity = new Vector3(input.x, 0f, input.y) * speed;
    }

    private void Update() {
        UpdateInputs();
        UpdateVelocity();
        ApplyLeashModifier();
        ApplyMinDistanceModifier();

        localRigidbody.position += velocity * Time.deltaTime;
    }
}
