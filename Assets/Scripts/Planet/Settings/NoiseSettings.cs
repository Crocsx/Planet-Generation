using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    [Range(1, 10)]
    [Tooltip("Number of noise layer")]
    public int layers = 1;
    [Tooltip("Control the amplitute of the noise")]
    public float strength = 1;
    [Tooltip("Initial Roughness value")]
    public float baseRoughness = 1;
    [Tooltip("Roughness incrementation per layer (higher => more detail)")]
    public float roughness = 2;
    [Tooltip("Amplitude decrementation value per layer")]
    public float persistance = 0.5f;
    [Tooltip("Minimum Elevation value")]
    public float minValue = 0.5f;
    public Vector3 center;
}
