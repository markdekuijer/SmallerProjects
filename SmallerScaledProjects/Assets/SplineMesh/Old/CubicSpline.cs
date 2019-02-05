using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicSpline : MonoBehaviour
{
    public Vector3[] positions = new Vector3[4];
    [Range(0,1)] public float t;

    Vector3 a;
    Vector3 b;
    Vector3 c;
    Vector3 d;
    Vector3 e;
    Vector3 f;

    private void Update()
    {
        a = Vector3.Lerp(positions[0], positions[1], t);
        b = Vector3.Lerp(positions[1], positions[2], t);
        c = Vector3.Lerp(positions[2], positions[3], t);
        d = Vector3.Lerp(a, b, t);
        e = Vector3.Lerp(b, c, t);
        f = Vector3.Lerp(d, e, t);
        t += Time.deltaTime * 0.25f;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(positions[0], positions[1]);
        Debug.DrawLine(positions[1], positions[2]);
        Debug.DrawLine(positions[2], positions[3]);
        Debug.DrawLine(a,b);
        Debug.DrawLine(b, c);
        Debug.DrawLine(d, e);
        Gizmos.DrawCube(f, Vector3.one * 0.1f);
    }
}
