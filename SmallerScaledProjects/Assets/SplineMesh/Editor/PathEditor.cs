﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path Path
    {
        get
        {
            return creator.path;
        }
    }

    const float segmentSelectDistanceThreshold = 0.1f;
    int selectedSegmentIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("CreateNew"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
        }

        bool isClosed = GUILayout.Toggle(Path.IsClosed, "is closed");
        if (isClosed != Path.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            Path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "auto set control points");
        if(autoSetControlPoints != Path.AutoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle auto set controls");
            Path.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
            SceneView.RepaintAll();
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if(selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split Segment");
                Path.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if(!Path.IsClosed)
            {
                Undo.RecordObject(creator, "Add Segment");
                Path.AddSegment(mousePos);
            }
        }

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = creator.anchorDiameter * 0.5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < Path.NumPoints; i+= 3)
            {
                float dst = Vector2.Distance(mousePos, Path[i]);
                if(dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if(closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete segment");
                Path.DeleteSegment(closestAnchorIndex);
            }

        }

        if(guiEvent.type == EventType.MouseMove)
        {
            float minDistanceToSegment = segmentSelectDistanceThreshold;
            int newSelectedSgementIndex = -1;

            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector3[] points = Path.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if(dst < minDistanceToSegment)
                {
                    minDistanceToSegment = dst;
                    newSelectedSgementIndex = i;
                }
            }

            if(newSelectedSgementIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSgementIndex;
                HandleUtility.Repaint();
            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    private void OnEnable()
    {
        creator = (PathCreator)target;

        if (creator.path == null)
            creator.CreatePath();
    }

    Vector3 lastPosInScene;
    void Draw()
    {
        for (int i = 0; i < Path.NumSegments; i++)
        {
            Vector3[] points = Path.GetPointsInSegment(i);
            if (creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[0], points[1]);
                Handles.DrawLine(points[2], points[3]);
            }

            Color segmentColor = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentColor : creator.segmentColor;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2);

            //TODO move editor spline with gameobject position
            //if(creator.gameObject.transform.position != lastPosInScene)
            //{
            //    Vector3 direction = creator.gameObject.transform.position - lastPosInScene;
            //    points[i] += direction;
            //}
        }


        for (int i = 0; i < Path.NumPoints; i++)
        {
            if(i%3==0 || creator.displayControlPoints)
            {
                Handles.color = (i % 3 == 0) ? creator.anchorColor : creator.controlColor;
                float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;

                Vector3 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);
                if(Path[i] != newPos)   
                {
                    Undo.RecordObject(creator, "Move point");
                    Path.MovePoint(i, newPos);
                }
            } 
        }

        lastPosInScene = creator.gameObject.transform.position;
    }
}