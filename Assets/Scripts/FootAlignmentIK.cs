using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FootAlignmentIK : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private TwoBoneIKConstraint leftFootIK;
    [SerializeField] private TwoBoneIKConstraint rightFootIK;
    [SerializeField] private bool useInterpolation = false;
    [SerializeField] private float interpolationSpeed = 6;
    [SerializeField] private float minCharacterHeight = 1.4f;
    [SerializeField] private Transform leftFootDebugObject;
    [SerializeField] private Transform rightFootDebugObject;
    [SerializeField] private Transform leftFootDefaultPlacement;
    [SerializeField] private Transform rightFootDefaultPlacement;

    private const float k_rayCastHeight = 0.5f;
    private const int k_groundLayer = 3;
    
    private Vector3 footOffset = new Vector3(0, 0.08f, 0);
    private Vector3 leftFootDebugSphere;
    private Vector3 rightFootDebugSphere;
    private float originalCharacterHeight;

    public bool IsWalking = false;
    public bool IsLeftFootDown = false;
    public bool IsRightFootDown = false;
    public float WeightInEffect = 1;
    
    public float LeftFootWeight = 0;
    public float RightFootWeight = 0;

    private void Awake()
    {
        originalCharacterHeight = characterController.height;
    }

    private void LateUpdate()
    {
        Transform leftFootTarget = leftFootIK.data.target;
        Transform rightFootTarget = rightFootIK.data.target;
        float speed = Time.deltaTime * interpolationSpeed;
        
        GetFootTargetPlacement(leftFootDefaultPlacement, true, out Vector3 leftFootPosition,
            out Quaternion leftFootRotation);
        GetFootTargetPlacement(rightFootDefaultPlacement, false, out Vector3 rightFootPosition,
            out Quaternion rightFootRotation);

        float newHeight = originalCharacterHeight -
                          (Mathf.Abs(leftFootTarget.position.y - rightFootTarget.position.y) + footOffset.y);
        newHeight = Mathf.Max(newHeight, minCharacterHeight);
        
        if (useInterpolation)
        {
            leftFootTarget.position = Vector3.Lerp(leftFootTarget.position, leftFootPosition, speed);
            rightFootTarget.position = Vector3.Lerp(rightFootTarget.position, rightFootPosition, speed);
            characterController.height = Mathf.Lerp(characterController.height, newHeight, speed);
            leftFootTarget.rotation = Quaternion.Lerp(leftFootTarget.rotation, leftFootRotation, speed);
            rightFootTarget.rotation = Quaternion.Lerp(rightFootTarget.rotation, rightFootRotation, speed);
        }
        else
        {
            leftFootTarget.position = leftFootPosition;
            rightFootTarget.position = rightFootPosition;
            characterController.height = newHeight;
            leftFootTarget.rotation = leftFootRotation;
            rightFootTarget.rotation = rightFootRotation;
        }
        
        leftFootIK.weight = WeightInEffect;
        rightFootIK.weight = WeightInEffect;
        
        if (IsWalking)
        {
            if (IsLeftFootDown)
            {
                leftFootTarget.position = leftFootDebugObject.position;
                LeftFootWeight = 1;
            }
            else
            {
                GetFootTargetPlacement(leftFootIK.data.tip, true, out leftFootPosition,
                out leftFootRotation);
                leftFootDebugObject.transform.position = leftFootPosition;
                LeftFootWeight = 0;
            }
            
            if (IsRightFootDown)
            {
                rightFootTarget.position = rightFootDebugObject.position;
                RightFootWeight = 1;
            }
            else
            {
                GetFootTargetPlacement(rightFootIK.data.tip, false, out rightFootPosition,
                    out rightFootRotation);
                rightFootDebugObject.transform.position = rightFootPosition;
                RightFootWeight = 0;
            }
            
            leftFootIK.weight = LeftFootWeight;
            rightFootIK.weight = RightFootWeight;
        }
    }

    private void GetFootTargetPlacement(Transform footIKTarget, bool isLeftFoot, out Vector3 position, out Quaternion rotation)
    {
        Vector3 footOrigin = footIKTarget.position + Vector3.up * k_rayCastHeight;
        RaycastHit hit;
        if (Physics.Raycast(footOrigin, Vector3.down, out hit, Mathf.Infinity, k_groundLayer))
        {
            Vector3 forward;
            Vector3 upwards;
            forward = Vector3.Cross(footIKTarget.right, hit.normal);
            upwards = Vector3.Cross(forward, footIKTarget.right);

            if (isLeftFoot)
            {
                rotation = Quaternion.LookRotation(-forward, -upwards);
            }
            else
            {
                rotation = Quaternion.LookRotation(forward, upwards);
                
            }
            position = hit.point + footOffset;
            if (isLeftFoot)
            {
                leftFootDebugSphere = hit.point;
            }
            else
            {
                rightFootDebugSphere = hit.point;
            }
        }
        else
        {
            position = footIKTarget.position;
            rotation = footIKTarget.rotation;
        }
    }
}
