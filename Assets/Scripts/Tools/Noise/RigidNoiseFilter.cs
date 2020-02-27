using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter: NoiseFilter
{
    protected NoiseSettings.RigidNoiseSettings settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings) : base()
    {
        this.settings = settings;
    }

    public override float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.layers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
            v *= v;

            // Region starting low down, will remain undetailed, compared to elevated region.
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistance;

        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}
