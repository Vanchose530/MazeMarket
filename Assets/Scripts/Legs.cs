using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Legs : MonoBehaviour
{
    [SerializeField] private Transform transform;
    [SerializeField] private SpatialSoundEffect stepSE;

    private void LegsSound()
    {
        AudioManager.instance.PlaySpatialSoundEffect(stepSE, transform);
    }
}
