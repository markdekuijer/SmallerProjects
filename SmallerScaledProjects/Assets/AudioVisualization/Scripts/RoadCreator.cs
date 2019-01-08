using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadCreator : MonoBehaviour
{
    [Range(0.005f, 1.5f)]
    public float spacing = 1;
    public float roadWidth = 1;
    public bool autoUpdate;
    public float tiling = 1;

    public float radius = 1;

    public void UpdateCylinderMesh()
    {
        Path path = GetComponent<PathCreator>().path;
        Vector3[] points = path.CalculateEvenlySpacedPoints(spacing);
        GetComponent<MeshFilter>().mesh = CreateCylinder(points, 9, path.IsClosed);

        int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * 0.05f);
        GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
    }

    Mesh CreateRoadMesh(Vector3[] points, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        int[] tris = new int[numTris * 3];
        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if(i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }
            forward.Normalize();
            Vector3 left = new Vector3(-forward.y, forward.x);

            verts[vertexIndex] = points[i] + left * roadWidth * 0.5f;
            verts[vertexIndex + 1] = points[i] - left * roadWidth * 0.5f;

            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            uvs[vertexIndex] = new Vector2(0, v);
            uvs[vertexIndex + 1] = new Vector2(1, v); 

            if(i < points.Length - 1 || isClosed)
            {
                tris[triangleIndex] = vertexIndex;
                tris[triangleIndex + 1] = (vertexIndex + 2) % verts.Length;
                tris[triangleIndex + 2] = vertexIndex + 1;

                tris[triangleIndex + 3] = vertexIndex + 1;
                tris[triangleIndex + 4] = (vertexIndex + 2) % verts.Length;
                tris[triangleIndex + 5] = (vertexIndex + 3) % verts.Length;
            }

            vertexIndex += 2;
            triangleIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        return mesh;
    }

    private Mesh CreateCylinder(Vector3[] points, int segments, bool isClosed)
    {
        Vector3[] verts = new Vector3[(points.Length * segments) + points.Length + (isClosed ? segments : 0)];
        Vector2[] uvs = new Vector2[verts.Length];
        List<int> tris = new List<int>();

        int vertexIndex = 0;
        int triangleIndex = 0;

        Vector3 up = Vector3.up;
        Vector3 forward = Vector3.zero;
        Vector3 right = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            forward = Vector3.zero;
            if (i < points.Length - 1)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            else
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }
            forward.Normalize();
            right = Vector3.Cross(Vector3.up, forward).normalized;
            up = Vector3.Cross(-right, forward).normalized;

            for (int j = 0; j < segments; j++)
            {
                float angle = 360f / ((float)segments - 1f); // 360 / 12 = 30
                float mathAngle = angle * j; // 30 * j = 0 30 60 90
                if (j == segments)
                {
                    verts[vertexIndex] = verts[vertexIndex - segments];
                }
                else
                {
                    verts[vertexIndex] = PointOnCircle(radius, mathAngle, points[i], forward, up);
                }

                float t = (float)j / (float)(segments - 1);
                float v = (float)i / (float)((isClosed ? points.Length : points.Length - 1));
                print(v);
                uvs[vertexIndex] = new Vector2(t, v);

                vertexIndex += 1;
            }

            if (i < points.Length - 1)
            {
                for (int j = 0; j < segments - 1; j++)
                {
                    tris.Add(triangleIndex);
                    tris.Add(triangleIndex + 1);
                    tris.Add(triangleIndex + segments);

                    tris.Add(triangleIndex + segments);
                    tris.Add(triangleIndex + 1);
                    tris.Add(triangleIndex + segments + 1);
                    triangleIndex += 1;
                }

                triangleIndex += 1;
            }
        }

        if (isClosed)
        {
            for (int j = 0; j < segments; j++)
            {
                float angle = 360f / ((float)segments - 1f); // 360 / 12 = 30
                float mathAngle = angle * j; // 30 * j = 0 30 60 90

                verts[vertexIndex] = verts[j];

                float t = (float)j / (float)(segments - 1);
                float v = 1;// (float)i / (float)(points.Length - 1);
                uvs[vertexIndex] = new Vector2(t, v);

                vertexIndex += 1;
            }

            for (int j = 0; j < segments - 1; j++)
            {
                tris.Add(triangleIndex);
                tris.Add(triangleIndex + 1);
                tris.Add(triangleIndex + segments);

                tris.Add(triangleIndex + segments);
                tris.Add(triangleIndex + 1);
                tris.Add(triangleIndex + segments + 1);
                triangleIndex += 1;
            }
        }
        print("verts | " + verts.Length + "    |||    tris | " + tris.ToArray().Length);

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs;
        mesh.name = "cylinder";
        return mesh;
    }

    public Vector3 PointOnCircle(float radius, float angleInDegrees, Vector3 origin, Vector3 forward, Vector3 up)
    {
        float x = (radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180F));
        float y = (radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180F));

        //Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        //Quaternion q = Quaternion.FromToRotation(Vector3.forward, forward);

        Vector3 toRotate = new Vector3(x, y, 0);
        Matrix4x4 mat = Matrix4x4.LookAt(Vector3.zero, forward, up);

        return mat.MultiplyPoint(toRotate) + origin;
    }
}
