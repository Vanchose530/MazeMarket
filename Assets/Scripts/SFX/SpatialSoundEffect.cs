using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpatialSoundEffect : SoundEffect
{
    [Header("Spatial Sound")]
    public float minDistance = 1;
    public float maxDistance = 15;
    public AudioRolloffMode volumeRolloff;
}
