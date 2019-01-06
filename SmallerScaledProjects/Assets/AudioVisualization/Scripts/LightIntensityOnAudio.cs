using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightIntensityOnAudio : MonoBehaviour
{
    [SerializeField] private AudioVisualizer visualizer;
    [SerializeField] private int band;
    [SerializeField] private float minIntensity, maxIntensity;
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.intensity = (visualizer.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
    }
}
