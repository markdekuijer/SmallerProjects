using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour
{

    public float spacing = .1f;
    public float resolution = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Placing");
            Vector3[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
            foreach (Vector2 p in points)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = p;
                g.transform.localScale = Vector3.one * spacing * 0.5f;
            }
        }

    }

}
