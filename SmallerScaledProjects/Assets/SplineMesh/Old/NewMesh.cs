using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class NewMesh : MonoBehaviour
{
    MeshFilter mf;
    Mesh m;

    Vector3[] vertices = new Vector3[]
    {
        new Vector3(1,0,1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,-1)
    };

    Vector3[] normals = new Vector3[]
    {
        new Vector3(0,1,0),
        new Vector3(0,1,0),
        new Vector3(0,1,0),
        new Vector3(0,1,0)
    };

    Vector2[] uvs = new Vector2[]
    {
        new Vector2(0,1),
        new Vector2(0,0),
        new Vector2(1,1),
        new Vector2(1,0),
    };

    int[] triangles = new int[]
    {
        0,2,3,
        3,1,0
    };


    private void Start()
    {
        mf = GetComponent<MeshFilter>();
        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();

        m = mf.sharedMesh;
    }

    public void CreateNewMesh()
    {
        m.Clear();
        m.vertices = vertices;
        m.normals = normals;
        m.uv = uvs;
        m.triangles = triangles;
    }

}
