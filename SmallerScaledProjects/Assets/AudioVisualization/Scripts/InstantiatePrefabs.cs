using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabs : MonoBehaviour
{
    [SerializeField] private AudioVisualizer visualizer;
    [SerializeField] private GameObject prefab;
    [SerializeField] float maxScale;
    private GameObject[] samplesOjbs = new GameObject[512];

    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.position = this.transform.position;
            g.transform.parent = this.transform;
            g.name = "cube " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            g.transform.position = Vector3.forward * 50;
            samplesOjbs[i] = g;
        }
    }

    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            samplesOjbs[i].transform.localScale = new Vector3(10, ((visualizer.samplesLeft[i] + visualizer.samplesRight[i]) / 2 * maxScale) + 2, 10);
        }
    }
}
