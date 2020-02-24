using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter: NoiseFilter
{
    protected NoiseSettings.SimpleNoiseSettings settings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings) : base()
    {
        this.settings = settings;
    }

    public override float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.layers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistance;

        }

        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
