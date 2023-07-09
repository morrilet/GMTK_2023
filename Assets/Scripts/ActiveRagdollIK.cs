using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollIK : MonoBehaviour
{
    protected Animator animator;

    public Transform rightHandTarget;
    public Transform lookTarget;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // public void SetRightHandTarget(Transform target) {
    //     rightHandTarget = target;
    // }

    private void OnAnimatorIK() {
        if (animator) {
            if (rightHandTarget != null) {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }

            if (lookTarget != null) {
                animator.SetLookAtWeight(1f);
                animator.SetLookAtPosition(lookTarget.position);
            }
        }
    }
}
