using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public Path path;
    public float movementSpeed;
    public float rotationSpeed;
    public float spacing = 0.005f;

    float progress = 0;
    float actualspeed;
    public Vector3[] points;

    int currentPoint;
    public string name;

    private void Start()
    {
        path = GameObject.Find(name).GetComponent<PathCreator>().path;
        points = path.CalculateEvenlySpacedPoints(spacing);
        print(points.Length);
        float distance = points.Length;
        actualspeed = 1 / distance;
    }

    private void Update()
    {
        progress += Time.deltaTime * movementSpeed;

        if (progress > 1f)
        {
            progress -= 1f;
            currentPoint++;

            if (currentPoint > points.Length - 1)
                currentPoint = 0;
        }
        transform.localPosition = Vector3.Lerp(points[currentPoint], points[CheckLooping(currentPoint + 1)], progress) + Vector3.down;

        Vector3 nextDir = points[CheckLooping10(currentPoint + 25)] - (transform.position + Vector3.up);
        Quaternion q = Quaternion.LookRotation(nextDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, rotationSpeed * Time.deltaTime);
    }

    public int CheckLooping(int i)
    {
        if (i > points.Length - 1)
            return 0;

        return i;
    }

    public int CheckLooping10(int i)
    {
        if (i > points.Length - 1)
        {
            return i - (points.Length - 1);
        }

        return i;
    }
}
