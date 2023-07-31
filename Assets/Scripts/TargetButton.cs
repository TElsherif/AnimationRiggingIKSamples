using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public GameObject BoneIKTarget;
    public GameObject LookIKTarget;
    public GameObject shoulderPosition;
    public float offsetDistance = 0.01f;

    void Update()
    {
        if (BoneIKTarget != null)
        {
            Vector3 position = transform.position;
            Vector3 targetOffset = (shoulderPosition.transform.position - position).normalized  * offsetDistance;
            BoneIKTarget.transform.position = position + targetOffset;
            
            Vector3 direction = position - BoneIKTarget.transform.position;
            BoneIKTarget.transform.rotation = Quaternion.LookRotation(direction);
            BoneIKTarget.transform.Rotate(70, 0, 0);
            
            LookIKTarget.transform.position = position;
        }
    }
}
