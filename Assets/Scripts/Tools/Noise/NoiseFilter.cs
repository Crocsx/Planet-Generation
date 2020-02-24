using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter : INoiseFilter
{
    protected Noise noise = new Noise();

    public virtual float Evaluate(Vector3 point)
    {
        return point.magnitude;
    }
}
