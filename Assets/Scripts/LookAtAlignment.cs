using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtAlignment : MonoBehaviour
{
    [SerializeField] private Transform aimObject;
    [SerializeField] private Transform targetHandle;
    [SerializeField] private TwoBoneIKConstraint handIK;
    [SerializeField] private TwoBoneIKConstraint headIK;
    [SerializeField] private TwoBoneIKConstraint chestIK;
    
    [SerializeField] private float chestIKWeight = 0.3f;
    [SerializeField] private float headIKWeight = 0.7f;
    [SerializeField] private float maxRotationAngle = 75f;

    [SerializeField] private Transform aimTarget;
    [SerializeField] private bool useCamera;
    [SerializeField] private Camera camera;
    
    private void LateUpdate()
    {
        Vector3 target = aimTarget.position;
        if (useCamera)
        {
            target = camera.transform.forward * 50f;
        }
        // Get the difference in rotation between the two transforms
        Quaternion rotationDifference = Quaternion.FromToRotation(transform.forward, target - transform.position);

        // Get the angle in degrees using the Quaternion.Angle method
        float magnitudeDifference = Quaternion.Angle(Quaternion.identity, rotationDifference);

        if (magnitudeDifference <= maxRotationAngle)
        {
            Vector3 chestLookDirection = target - chestIK.data.target.position;
            Quaternion chestRotation = Quaternion.LookRotation(chestLookDirection);
            chestIK.weight = chestIKWeight;

            Vector3 headLookDirection = target - headIK.data.target.position;
            Quaternion headRotation = Quaternion.LookRotation(headLookDirection);
            headIK.weight = headIKWeight;

            handIK.data.target.position = targetHandle.position;
            handIK.data.target.rotation = targetHandle.rotation;

            chestIK.data.target.rotation = chestRotation;
            headIK.data.target.rotation = headRotation;
            aimObject.rotation = GetLookRotationWithFixedZ(aimObject, target);
        }
    }

    private Quaternion GetLookRotationWithFixedZ(Transform transformObject, Vector3 target)
    {
        // Store the original z-axis rotation of the object
        float originalZRotation = transform.eulerAngles.z;
        
        // Get the direction from this object to the target
        Vector3 directionToTarget = target - transformObject.position;

        // Calculate the rotation that would look at the target
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        // Convert the look rotation to Euler angles
        Vector3 eulerAngles = lookRotation.eulerAngles;

        // Maintain the original z-axis rotation
        eulerAngles.z = originalZRotation;

        // Apply the new rotation
        return Quaternion.Euler(eulerAngles);
    }
}
