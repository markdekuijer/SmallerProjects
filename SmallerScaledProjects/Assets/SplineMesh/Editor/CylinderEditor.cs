﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CylinderCreator))]
public class CylinderEditor : Editor
{
    CylinderCreator creator;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Repaint"))
        {
            Debug.ClearDeveloperConsole();
            creator.UpdateCylinderMesh();
        }
    }

    private void OnEnable()
    {
        creator = (CylinderCreator)target;
    }
}
