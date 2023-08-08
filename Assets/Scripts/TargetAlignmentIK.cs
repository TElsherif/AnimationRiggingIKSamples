using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TargetAlignmentIK : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint boneIK;
    [SerializeField] private Transform targetHandle;

    private void LateUpdate()
    {
        // boneIK.data.target.position = targetHandle.position;
        // boneIK.data.target.rotation = targetHandle.rotation;
    }
}
