using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadCreator))]
public class RoadEditor : Editor
{
    RoadCreator creator;

    private void OnSceneGUI()
    {
        if (creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateRoadMesh();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("I am a regular Automatic Layout Button"))
        {
            creator.UpdateRoadMesh();
        }
    }

    private void OnEnable()
    {
        creator = (RoadCreator)target;
    }
}
