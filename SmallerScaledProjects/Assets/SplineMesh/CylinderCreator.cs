using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CylinderCreator : MonoBehaviour
{
    public Vector3[] points = new Vector3[2];
    [Range(8,360)] public int vertsCircle = 12;
    public float radius = 1;

    public List<Vector3> from;
    public List<Vector3> towards;
    public List<Vector3> leftTowards;
    public List<Vector3> ups;

    public void UpdateCylinderMesh()
    {
        from.Clear();
        towards.Clear();
        leftTowards.Clear();
        ups.Clear();
        GetComponent<MeshFilter>().mesh = CreateCylinder(points, vertsCircle);

        //int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * 0.05f);
        //GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(from[i], towards[i] + from[i], Color.red);
            Debug.DrawLine(from[i], leftTowards[i] + from[i], Color.blue);
            Debug.DrawLine(from[i], ups[i] + from[i], Color.yellow);
        }
    }

    private Mesh CreateCylinder(Vector3[] points, int segments)
    {
        Vector3[] verts = new Vector3[(points.Length * segments) + points.Length];
        Vector2[] uvs = new Vector2[verts.Length];
        List<int> tris = new List<int>();

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            from.Add(points[i]);
            Vector3 forward = Vector3.zero;
            if (i < points.Length - 1)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            else
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }
            forward.Normalize();
            towards.Add(forward);
            Vector3 left = Vector3.Cross(Vector3.up, forward).normalized;
            Vector3 up = Vector3.Cross(-left, forward).normalized;
            leftTowards.Add(left);
            ups.Add(up);

            for (int j = 0; j < segments; j++)
            {
                float angle = 360f / ((float)segments - 1f); // 360 / 12 = 30
                float mathAngle = angle * j; // 30 * j = 0 30 60 90
                if(j == segments)
                {
                    verts[vertexIndex] = verts[vertexIndex - segments];
                }
                else
                {
                    verts[vertexIndex] = PointOnCircle(radius, mathAngle, points[i], forward, up);
                }

                float t = (float)j / (float)(segments - 1);
                float v = (float)i / (float)(points.Length - 1);
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
        float x = (radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180F));// + origin.x;
        float y = (radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180F));// + origin.y;
        Vector3 toRotate = new Vector3(x, y, 0);// origin.z);
        //Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
//        Quaternion q = Quaternion.FromToRotation(Vector3.forward, forward);
        
        Matrix4x4 mat = Matrix4x4.LookAt(Vector3.zero, forward, up);
                
        return mat.MultiplyPoint(toRotate) + origin;
    }
}
