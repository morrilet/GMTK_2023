using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActiveRagdoll : MonoBehaviour
{
    [SerializeField] protected GameObject animatedCharacter;
    [SerializeField] protected GameObject ragdollCharacter;
    [SerializeField] protected ConfigurableJoint balanceJoint;

    [Space, Header("Balanced")]
    [SerializeField] protected float balanceStrength = 100f;
    [SerializeField] protected float balanceDamper = 10f;
    [SerializeField] protected float springStrength = 1000f;
    [SerializeField] protected float springDamper = 10f;

    [Space, Header("Unbalanced")]
    [SerializeField] protected float balanceStrengthLow = 50f;
    [SerializeField] protected float balanceDamperLow = 10f;
    [SerializeField] protected float springStrengthLow = 500f;
    [SerializeField] protected float springDamperLow = 5f;

    // [Space, Header("Turning")]
    // [SerializeField] protected float turnStrength = 100f;
    // [SerializeField] protected float turnDamper = 10f;

    protected Transform[] animatedTransforms;
    protected ConfigurableJoint[] ragdollJoints;
    protected Quaternion[] ragdollJointStartRotations;

    private bool syncBody = true;

    public enum BalanceState {
        Balanced,
        Unbalanced,
        Falling
    }

    protected void InitializeBodyData() {
        animatedTransforms = animatedCharacter.GetComponentsInChildren<Transform>();
        ragdollJoints = ragdollCharacter.GetComponentsInChildren<ConfigurableJoint>().Where(joint => joint.gameObject != balanceJoint.gameObject).ToArray();
        ragdollJointStartRotations = new Quaternion[ragdollJoints.Length];
        for (int i = 0; i < ragdollJoints.Length; i++) {
            ragdollJointStartRotations[i] = ragdollJoints[i].transform.localRotation;
            ragdollJoints[i].enablePreprocessing = false;
            ragdollJoints[i].SetupAsCharacterJoint();
        }
    }

    protected void UpdateSpringStrength(float strength, float damper) {
        for (int i = 0; i < ragdollJoints.Length; i++) {
            JointDrive slerpDrive = ragdollJoints[i].slerpDrive;
            slerpDrive.positionSpring = strength;
            slerpDrive.positionDamper = damper;
            ragdollJoints[i].slerpDrive = slerpDrive;
        }
    }

    protected void UpdateBalanceStrength(float strength, float damper) {
        JointDrive slerpDrive = balanceJoint.slerpDrive;
        slerpDrive.positionSpring = strength;
        slerpDrive.positionDamper = damper;
        balanceJoint.slerpDrive = slerpDrive;
    }

    public void SetBalanceState(BalanceState state) {
        switch (state) {
            case BalanceState.Balanced:
                SetBalanced();
                break;
            case BalanceState.Unbalanced:
                SetUnbalanced();
                break;
            case BalanceState.Falling:
                SetFalling();
                break;
            default:
                Debug.LogWarning("Unknown BalanceState: " + state.ToString());
                break;
        }
    }

    private void SetBalanced() {
        balanceJoint.xMotion = ConfigurableJointMotion.Locked;
        balanceJoint.yMotion = ConfigurableJointMotion.Locked;
        balanceJoint.zMotion = ConfigurableJointMotion.Locked;
        UpdateSpringStrength(springStrength, springDamper);
        UpdateBalanceStrength(balanceStrength, balanceDamper);
        syncBody = true;
    }

    private void SetUnbalanced() {
        balanceJoint.xMotion = ConfigurableJointMotion.Locked;
        balanceJoint.yMotion = ConfigurableJointMotion.Locked;
        balanceJoint.zMotion = ConfigurableJointMotion.Free;
        UpdateSpringStrength(springStrengthLow, springDamperLow);
        UpdateBalanceStrength(balanceStrengthLow, balanceDamperLow);
        syncBody = true;
    }

    private void SetFalling() {
        balanceJoint.xMotion = ConfigurableJointMotion.Locked;
        balanceJoint.yMotion = ConfigurableJointMotion.Locked;
        balanceJoint.zMotion = ConfigurableJointMotion.Free;
        UpdateSpringStrength(0.0f, 0.0f);
        UpdateBalanceStrength(0.0f, 0.0f);
        syncBody = false;
    }

    protected void Awake() {
        InitializeBodyData();
        SetBalanceState(BalanceState.Falling);
    }

    protected void FixedUpdate() {
        if (syncBody)
            SyncBodyData();
    }

    public void TurnTowardsTarget(Vector3 target) {
        animatedCharacter.transform.LookAt(target);
    }

    protected void SyncBodyData() {
        for (int i = 0; i < animatedTransforms.Length; i++) {
            for (int j = 0; j < ragdollJoints.Length; j++) {
                if (animatedTransforms[i].gameObject.name == ragdollJoints[j].gameObject.name) {
                    Vector3 euler = animatedTransforms[i].localRotation.eulerAngles;
                    euler = new Vector3(Mathf.Round(euler.x), Mathf.Round(euler.y), Mathf.Round(euler.z));
                    Quaternion targetRot = Quaternion.Euler(euler);
                    ragdollJoints[j].SetTargetRotationLocal(targetRot, ragdollJointStartRotations[j]);
                }
            }
        }
    }
}
