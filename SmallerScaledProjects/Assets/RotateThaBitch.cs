using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThaBitch : MonoBehaviour
{
    public float rotateSpeed;
    public Transform target;
	void Update ()
    {
        //transform.LookAt(target.position);
        //----------------
        Vector3 nextDir = transform.position - target.position;
        Quaternion q = Quaternion.LookRotation(nextDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotateSpeed * Time.deltaTime);
    }
}
