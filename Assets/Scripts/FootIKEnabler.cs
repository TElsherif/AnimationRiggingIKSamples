using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FootAlignmentIK))]
[RequireComponent(typeof(CharacterController))]
public class FootIKEnabler : MonoBehaviour
{
    [SerializeField] private FootAlignmentIK footIK;
    [SerializeField] private CharacterController characterController;

    private void Awake()
    {
        if (footIK == null)
        {
            footIK = GetComponent<FootAlignmentIK>();
        }
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        Vector3 xzVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        float speed = xzVelocity.magnitude;
        
        if (speed < 0.5f && characterController.isGrounded)
        {
            footIK.WeightInEffect = 1 - speed;
        }
        else 
        {
            footIK.WeightInEffect = 0;
        }
    }
}
