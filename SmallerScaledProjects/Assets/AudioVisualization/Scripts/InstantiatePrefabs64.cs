﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabs64 : MonoBehaviour
{
    [SerializeField] private AudioVisualizer visualizer;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float maxScale;
    [SerializeField] private bool useBuffer;
    private GameObject[] samplesOjbs = new GameObject[64];

    void Start()
    {
        for (int i = 0; i < 64; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.position = this.transform.position;
            g.transform.parent = this.transform;
            g.name = "cube " + i;
            Vector3 v = Vector3.right * i;
            v.x -= 32;
            g.transform.position = v;
            samplesOjbs[i] = g;
        }
    }

    void Update()
    {
        for (int i = 0; i < 64; i++)
        {
            if(useBuffer)
                samplesOjbs[i].transform.localScale = new Vector3(10, (visualizer.audioBandBuffer64[i] * maxScale) + 2, 10);
            else
                samplesOjbs[i].transform.localScale = new Vector3(10, (visualizer.audioBand64[i] * maxScale) + 2, 10);
        }
    }
}
