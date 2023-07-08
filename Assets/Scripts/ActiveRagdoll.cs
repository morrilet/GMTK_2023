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
    [SerializeField] protected float balanceStrength = 100f;

    protected Transform[] animatedTransforms;
    protected ConfigurableJoint[] ragdollJoints;
    protected Quaternion[] ragdollJointStartRotations;

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

    protected void UpdateSpringStrength() {
        for (int i = 0; i < ragdollJoints.Length; i++) {
            JointDrive xDrive = ragdollJoints[i].angularXDrive;
            JointDrive yzDrive = ragdollJoints[i].angularYZDrive;

            xDrive.positionSpring = springStrength;
            yzDrive.positionSpring = springStrength;

            ragdollJoints[i].angularXDrive = xDrive;
            ragdollJoints[i].angularYZDrive = yzDrive;
        }
    }

    protected void UpdateBalanceStrength() {
        JointDrive xDrive = balanceJoint.angularXDrive;
        JointDrive yzDrive = balanceJoint.angularYZDrive;

        xDrive.positionSpring = balanceStrength;
        yzDrive.positionSpring = balanceStrength;

        balanceJoint.angularXDrive = xDrive;
        balanceJoint.angularYZDrive = yzDrive;
    }

    // protected void SetBalanceLockState(bool locked) {
    //     balanceJoint.angularXMotion = state ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Free;
    //     balanceJoint.angularYMotion = state ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Free;
    //     balanceJoint.angularZMotion = state ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Free;
    // }

    protected void Awake() {
        InitializeBodyData();
        UpdateSpringStrength();
        UpdateBalanceStrength();
    }

    protected void FixedUpdate() {
        SyncBodyData();

        // For testing...
        UpdateSpringStrength();
        UpdateBalanceStrength();
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

    protected void SetBalanceState(BalanceState state) {
        // switch (state))
        // {
        //     case BalanceState.Balanced:
        //         balanceStrength = 100f;
        //         break;
                
        //     default:
        // }
    }
}
