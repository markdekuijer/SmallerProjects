using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchSelfToSplineWalker : MonoBehaviour
{
    public Transform splineWalker;

    private void Start()
    {
        transform.parent = splineWalker;
    }
}
