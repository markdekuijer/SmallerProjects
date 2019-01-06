using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    [SerializeField] private AudioVisualizer visualizer;
    [SerializeField] private float startScale, maxScale;
    [SerializeField] private bool useBuffer;

    [SerializeField] private float red, green, blue;
    Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3((visualizer.AmplitudeBuffer * maxScale) + startScale, (visualizer.AmplitudeBuffer * maxScale) + startScale, (visualizer.AmplitudeBuffer * maxScale) + startScale);
            Color c = new Color(red * visualizer.AmplitudeBuffer, green * visualizer.AmplitudeBuffer, blue * visualizer.AmplitudeBuffer);
            material.SetColor("_EmissionColor", c);
        }
        else
        {
            transform.localScale = new Vector3((visualizer.Amplitude * maxScale) + startScale, (visualizer.Amplitude * maxScale) + startScale, (visualizer.Amplitude * maxScale) + startScale);
            Color c = new Color(red * visualizer.Amplitude, green * visualizer.Amplitude, blue * visualizer.Amplitude);
            material.SetColor("_EmissionColor", c);
        }
    }
}
