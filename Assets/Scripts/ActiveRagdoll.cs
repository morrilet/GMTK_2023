using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActiveRagdoll : MonoBehaviour
{
    [SerializeField] protected GameObject animatedCharacter;
    [SerializeField] protected GameObject ragdollCharacter;
    [SerializeField] protected ConfigurableJoint balanceJoint;
    [SerializeField] protected float springStrength = 1000f;
    [SerializeField] protected float springStrengthLow = 500f;
    [SerializeField] protected float balanceStrength = 100f;
    [SerializeField] protected float balanceStrengthLow = 50f;

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
            ragdollJoints[i].SetupAsCharacterJoint();
        }
    }

    protected void UpdateSpringStrength(float strength) {
        for (int i = 0; i < ragdollJoints.Length; i++) {
            JointDrive xDrive = ragdollJoints[i].angularXDrive;
            JointDrive yzDrive = ragdollJoints[i].angularYZDrive;

            xDrive.positionSpring = strength;
            yzDrive.positionSpring = strength;

            ragdollJoints[i].angularXDrive = xDrive;
            ragdollJoints[i].angularYZDrive = yzDrive;
        }
    }

    protected void UpdateBalanceStrength(float strength) {
        JointDrive xDrive = balanceJoint.angularXDrive;
        JointDrive yzDrive = balanceJoint.angularYZDrive;

        xDrive.positionSpring = strength;
        yzDrive.positionSpring = strength;

        balanceJoint.angularXDrive = xDrive;
        balanceJoint.angularYZDrive = yzDrive;
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
        UpdateSpringStrength(springStrength);
        UpdateBalanceStrength(balanceStrength);
        syncBody = true;
    }

    private void SetUnbalanced() {
        balanceJoint.xMotion = ConfigurableJointMotion.Locked;
        balanceJoint.yMotion = ConfigurableJointMotion.Locked;
        balanceJoint.zMotion = ConfigurableJointMotion.Free;
        UpdateSpringStrength(springStrengthLow);
        UpdateBalanceStrength(balanceStrengthLow);
        syncBody = true;
    }

    private void SetFalling() {
        balanceJoint.xMotion = ConfigurableJointMotion.Locked;
        balanceJoint.yMotion = ConfigurableJointMotion.Locked;
        balanceJoint.zMotion = ConfigurableJointMotion.Free;
        UpdateSpringStrength(0.0f);
        UpdateBalanceStrength(0.0f);
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
                    ragdollJoints[j].SetTargetRotationLocal(animatedTransforms[i].localRotation, ragdollJointStartRotations[j]);
                }
            }
        }
    }
}
