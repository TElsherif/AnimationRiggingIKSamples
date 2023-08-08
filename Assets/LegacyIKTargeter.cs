using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyIKTargeter : MonoBehaviour
{
    public Transform leftHandTarget;   // Assign the target GameObject for the left hand
    public Transform rightHandTarget;  // Assign the target GameObject for the right hand

    private Animator animator;         // Reference to the Animator component
    private Transform leftHand;        // Reference to the left hand bone
    private Transform rightHand;       // Reference to the right hand bone

    private void Start()
    {
        // Get references to the Animator component and hand bones
        animator = GetComponent<Animator>();
        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
    }

    private void LateUpdate()
    {
        if (animator != null && leftHand != null && rightHand != null)
        {
            // Apply IK position to the hands
            if (leftHandTarget != null)
            {
                ApplyIK(leftHand, leftHandTarget);
            }

            if (rightHandTarget != null)
            {
                ApplyIK(rightHand, rightHandTarget);
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // Set the right hand target position and rotation, if one has been assigned
        if (rightHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }

        // Set the right hand target position and rotation, if one has been assigned
        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }

    private void ApplyIK(Transform handBone, Transform target)
    {
        // Calculate the IK position using legacy IK solver
        Vector3 targetPosition = target.position;
        Quaternion targetRotation = target.rotation;

        // Apply IK position to the hand bone
        handBone.position = targetPosition;
        handBone.rotation = targetRotation;
    }
}
