using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundEffect
{
    public AudioClip sound;
    [Range(0, 1f)]
    public float volume = 0.5f;
    [Range(0, 2f)]
    public float minPitch = 1f;
    [Range(0, 2f)]
    public float maxPitch = 1f;
}
