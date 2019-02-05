using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { ridged, simple };
    public FilterType filterType; 

    public float strenght = 1;
    [Range(1,8)] public int numLayers = 1;
    public float baseRoughness = 1;
    public float roughness = 1;
    public float percistance = 0.5f;
    public Vector3 center;
    public float minValue;
}
