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

    protected void InitializeBodyData() {
        animatedTransforms = animatedCharacter.GetComponentsInChildren<Transform>();
        ragdollJoints = ragdollCharacter.GetComponentsInChildren<ConfigurableJoint>().Where(joint => joint.gameObject != balanceJoint.gameObject).ToArray();
        ragdollJointStartRotations = new Quaternion[ragdollJoints.Length];
        for (int i = 0; i < ragdollJoints.Length; i++) {
            ragdollJointStartRotations[i] = ragdollJoints[i].transform.localRotation;
            ragdollJoints[i].SetupAsCharacterJoint();
        }
        UpdateSpringStrength();
        UpdateBalanceStrength();
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

    protected void SyncBodyData() {
        for (int i = 0; i < animatedTransforms.Length; i++) {
            for (int j = 0; j < ragdollJoints.Length; j++) {
                if (animatedTransforms[i].gameObject.name == ragdollJoints[j].gameObject.name) {
                    ragdollJoints[j].SetTargetRotationLocal(animatedTransforms[i].localRotation, ragdollJointStartRotations[j]);
                }
            }
        }
    }

    // protected void ApplyBalanceForces(BalanceMode mode) {
    //     if (mode == BalanceMode.STABLE) {
    //         BalanceStable();
    //     }
    // }

    // protected void BalanceStable() {
    //     Quaternion targetRotation = Quaternion.FromToRotation(-ragdollBalanceRoot.transform.forward, Vector3.up);
    //     Debug.DrawRay(ragdollBalanceRoot.transform.position, targetRotation.eulerAngles, Color.red, 0.01f);
    //     ragdollBalanceRoot.AddTorque(targetRotation.eulerAngles * balanceStrength);

    //     // Point the balance root towards the target direction.
    //     // TODO: Replace Vector3.forward with the direction of the character's movement.

    //     // float angle = Vector3.SignedAngle(ragdollBalanceRoot.transform.forward, Vector3.forward, Vector3.up) / 180f;
    //     // float percent = stableBalanceCurve.Evaluate(angle);
    //     // ragdollBalanceRoot.AddRelativeTorque(0, percent * balanceStrength, 0);
    // }

    protected void Awake() {
        InitializeBodyData();
    }

    protected void FixedUpdate() {
        SyncBodyData();
        UpdateSpringStrength();
        UpdateBalanceStrength();
        // ApplyBalanceForces(balanceMode);
    }
}
