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

    public void UpdateCylinderMesh()
    {
        GetComponent<MeshFilter>().mesh = CreateCylinder(points, vertsCircle);

        //int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * 0.05f);
        //GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
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
            //Vector3 forward = Vector3.zero;
            //if (i < points.Length - 1)
            //{
            //    forward += points[(i + 1) % points.Length] - points[i];
            //}
            //if (i > 0)
            //{
            //    forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            //}
            //forward.Normalize();
            //Vector3 left = new Vector3(-forward.y, forward.x);

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
                    print(vertexIndex);
                    verts[vertexIndex] = PointOnCircle(radius, mathAngle, points[i]);
                }


                float t = (float)j / (float)(segments - 1);
                //Debug.Log(j + " " + t);
                float v = (float)i / (float)(points.Length - 1);
                uvs[vertexIndex] = new Vector2(t, v);

                vertexIndex += 1;
            }

            if (i < points.Length - 1)
            {
                for (int j = 0; j < segments - 1; j++)
                {
                    /*if (j == segments - 1)
                    {
                        tris.Add(triangleIndex);
                        tris.Add(triangleIndex - (segments - 1));
                        tris.Add(triangleIndex + segments);

                        tris.Add(triangleIndex + segments);
                        tris.Add(triangleIndex - (segments - 1));
                        tris.Add(triangleIndex + 1);
                    }
                    else*/
                    {
                        tris.Add(triangleIndex);
                        tris.Add(triangleIndex + 1);
                        tris.Add(triangleIndex + segments);

                        tris.Add(triangleIndex + segments);
                        tris.Add(triangleIndex + 1);
                        tris.Add(triangleIndex + segments + 1);
                    }
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

    public Vector3 PointOnCircle(float radius, float angleInDegrees, Vector3 origin)
    {
        float x = (radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180F)) + origin.x;
        float y = (radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180F)) + origin.y;

        return new Vector3(x, y, origin.z);
    }
}
