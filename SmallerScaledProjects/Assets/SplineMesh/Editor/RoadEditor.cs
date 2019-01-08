using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(RoadCreator))]
public class RoadEditor : Editor
{
    RoadCreator creator;

    private void OnSceneGUI()
    {
        if (creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateCylinderMesh();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("I am a regular Automatic Layout Button"))
        {
            creator.UpdateCylinderMesh();
        }
    }

    private void OnEnable()
    {
        creator = (RoadCreator)target;
    }
}
