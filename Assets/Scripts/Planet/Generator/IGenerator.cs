using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerator<T>
{
    void UpdateSettings(T settings);
}
