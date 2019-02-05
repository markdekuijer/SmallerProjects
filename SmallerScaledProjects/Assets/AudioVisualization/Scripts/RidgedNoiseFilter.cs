 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgedNoiseFilter: INoiseFilter
{
    NoiseSettings settings;
    Noise noise = new Noise();

    public RidgedNoiseFilter(NoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequenty = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequenty + settings.center));
            v *= v;
            v *= weight;
            weight = v;
            noiseValue += v * amplitude;
            frequenty *= settings.roughness;
            amplitude *= settings.percistance;

        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strenght;
    }
}
