using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAudio : MonoBehaviour
{
    [SerializeField] private AudioVisualizer visualizer;
    [SerializeField] private int band;
    [SerializeField] private float startScale, scaleMultiplier;
    [SerializeField] private bool useBuffer;
    private Material m;

    private void Start()
    {
        m = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (visualizer.audioBandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
            Color c = new Color(visualizer.audioBandBuffer[band], visualizer.audioBandBuffer[band], visualizer.audioBandBuffer[band]);
            m.SetColor("_EmissionColor", c);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (visualizer.audioBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
            Color c = new Color(visualizer.audioBand[band], visualizer.audioBand[band], visualizer.audioBand[band]);
            m.SetColor("_EmissionColor", c);
        }
    }
}
