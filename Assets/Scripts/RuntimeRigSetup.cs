using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class IKBoneRigSetup : MonoBehaviour
{
    public List<Transform> boneTips;
    public bool createHints = true;
    public RigEffectorData.Style targetEffectorStyle = new ()
    {
        color = Color.green,
        size = 0.05f,
    };

    public RigEffectorData.Style hintEffectorStyle  = new ()
    {
        color = Color.yellow,
        size = 0.05f,
    };
    
    private readonly List<Transform> _targets = new List<Transform>();
    private readonly List<Transform> _hints = new List<Transform>();

    public void SetupNewRig()
    {
        RemoveRigSetup();
        GameObject rootRigGO = new GameObject("Runtime_Rig");
        rootRigGO.transform.SetParent(transform);
        rootRigGO.transform.position = transform.position;
        
        // create rig with all its child constraints
        Rig rootRig = rootRigGO.AddComponent<Rig>();
        foreach (Transform bone in boneTips)
        {
            Transform parent = bone.parent;

            if (!parent || !parent.parent)
            {
                Debug.LogError(bone.name + " does not have a proper bone heirarchy. Please check the parents.");
                return;
            }
            
            GameObject rigGO = new GameObject(bone.name + "_IK");
            GameObject targetGO = new GameObject(bone.name + "_Target");
            rigGO.transform.SetParent(rootRigGO.transform);
            targetGO.transform.SetParent(rigGO.transform);
            

            TwoBoneIKConstraint ikConstraint = rigGO.AddComponent<TwoBoneIKConstraint>();
            ikConstraint.data.root = parent.parent;
            ikConstraint.data.mid = parent;
            ikConstraint.data.tip = bone;
            ikConstraint.data.target = targetGO.transform;

            rigGO.transform.rotation = bone.rotation;
            rigGO.transform.position = bone.position;
            _targets?.Add(targetGO.transform);
            
            if (createHints)
            {
                GameObject hintGO = new GameObject(bone.name + "_Hint");
                hintGO.transform.SetParent(rigGO.transform);
                ikConstraint.data.hint = hintGO.transform;
                hintGO.transform.rotation = parent.rotation;
                hintGO.transform.position = parent.position;
                _hints?.Add(hintGO.transform);
            }
        }
        
        // add rig builder and add its rig layer
        RigBuilder rigBuilder = gameObject.GetComponent<RigBuilder>();
        if (rigBuilder == null)
        {
            rigBuilder = gameObject.AddComponent<RigBuilder>();
        }
        rigBuilder.layers.Add(new RigLayer(rootRig));
    }

    public void VisualizeRig()
    {
        // visualize rigged bones
        int boneCount = boneTips.Count * 3;
        int index = 0;
        Transform[] boneTransforms = new Transform[boneCount];
        foreach (Transform bone in boneTips)
        {
            Transform parent = bone.parent;
            if (!parent || !parent.parent)
            {
                Debug.LogError(bone.name + " does not have a proper bone heirarchy. Please check the parents.");
                return;
            }
            
            boneTransforms[index++] = bone;
            boneTransforms[index++] = parent;
            boneTransforms[index++] = parent.parent;
        }
        
        BoneRenderer boneRenderer = gameObject.GetComponent<BoneRenderer>();
        if (boneRenderer == null)
        {
            boneRenderer = gameObject.AddComponent<BoneRenderer>();
        }
        boneRenderer.transforms = boneTransforms;
        
        // visualize effectors
        RigBuilder rigBuilder = GetComponentInChildren<RigBuilder>();
        if (rigBuilder != null)
        {
            foreach (var target in _targets)
            {
                rigBuilder.AddEffector(target, targetEffectorStyle);
            }
            foreach (var hint in _hints)
            {
                rigBuilder.AddEffector(hint, hintEffectorStyle);
            }
        }
    }
    
    public void RemoveRigSetup()
    {
        _targets?.Clear();
        _hints?.Clear();

        RigBuilder rigBuilder = gameObject.GetComponent<RigBuilder>();
        if (rigBuilder != null)
        {
            rigBuilder.Clear();
            rigBuilder.layers.Clear();
        }

        BoneRenderer boneRenderer = gameObject.GetComponent<BoneRenderer>();
        if (boneRenderer != null)
        {
            boneRenderer.transforms = null;
        }
        
        Rig[] rigs = GetComponentsInChildren<Rig>();
        foreach (var rig in rigs)
        {
            if (rig != null)
            {
                DestroyImmediate(rig.gameObject);
            }
        }
    }

    public void HideVisuals()
    {
        BoneRenderer boneRenderer = gameObject.GetComponent<BoneRenderer>();
        if (boneRenderer != null)
        {
            DestroyImmediate(boneRenderer);
        }
        
        RigBuilder rigBuilder = GetComponent<RigBuilder>();
        if (rigBuilder != null)
        {
            foreach (var target in _targets)
            {
                rigBuilder.RemoveEffector(target);
            }
            foreach (var hint in _hints)
            {
                rigBuilder.RemoveEffector(hint);
            }
        }
    }
}
