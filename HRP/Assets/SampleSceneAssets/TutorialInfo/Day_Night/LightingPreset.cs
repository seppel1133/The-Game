using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightingPreset", menuName = "HRP/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject {
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
    public Gradient FogIntensity;

}
