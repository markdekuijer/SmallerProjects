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
        GetComponent<MeshFilter>().mesh = CreateCylinder(points, path.IsClosed);

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

    Mesh CreateCylinder(Vector3[] points, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * 12];
        //Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 12 * (points.Length - 1) + ((isClosed) ? 12 : 0);
        int[] tris = new int[numTris * 3];
        int vertexIndex = 0;
        int triangleIndex = 0;

        /* 
        CylinderCode (get vert could frist)
        for(int i = 0; i < vertices; i++)
        {
            polyList.add(circleVertices1[i], circleVertices1[(i+1) % vertices], circleVertices2[i]);
            polyList.add(circleVertices2[i], circleVertices2[(i+1) % vertices], circleVertices1[(i+1) % vertices]);
        }

        point on circle
        x = cx + r * cos(a)
        y = cy + r * sin(a)
        */

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }
            forward.Normalize();
            Vector3 left = new Vector3(-forward.y, forward.x);

            for (int j = 0; j < 4; j++)
            {
                float angle = 90 * i;
                Vector2 location = Vector2.zero;
                location.x = points[i].x + radius * Mathf.Cos(angle);
                location.y = points[i].z + radius * Mathf.Sin(angle);

                verts[vertexIndex] = points[i];
                verts[vertexIndex + 1] = points[i] + new Vector3(location.x,0,location.y);
                vertexIndex += 2;
            }


            //float completionPercent = i / (float)(points.Length - 1);
            //float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            //uvs[vertexIndex] = new Vector2(0, v);
            //uvs[vertexIndex + 1] = new Vector2(1, v);

            //if (i < points.Length - 1 || isClosed)
            //{
            //    tris[triangleIndex] = vertexIndex;
            //    tris[triangleIndex + 1] = (vertexIndex + 2) % verts.Length;
            //    tris[triangleIndex + 2] = vertexIndex + 1;

            //    tris[triangleIndex + 3] = vertexIndex + 1;
            //    tris[triangleIndex + 4] = (vertexIndex + 2) % verts.Length;
            //    tris[triangleIndex + 5] = (vertexIndex + 3) % verts.Length;
            //}

            //triangleIndex += 6;
        }

        for (int i = 0; i < verts.Length; i++)
        {
            tris[0] = i;
            tris[0 + 1] = i + 1;
            tris[0 + 2] = i + 2;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        //mesh.uv = uvs;

        return mesh;
    }
}
