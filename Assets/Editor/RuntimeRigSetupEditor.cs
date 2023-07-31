using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IKBoneRigSetup))]
public class RuntimeRigSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IKBoneRigSetup runtimeRigSetup = (IKBoneRigSetup)target;
        if(GUILayout.Button("Setup Rig"))
        {
            runtimeRigSetup.SetupNewRig();
        }
        
        if(GUILayout.Button("Visualize Rig"))
        {
            runtimeRigSetup.VisualizeRig();
        }
    }
}
