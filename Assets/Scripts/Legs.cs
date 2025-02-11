using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Legs : MonoBehaviour
{
    [SerializeField] private Transform transform;
    [SerializeField] private SoundEffect stepSE;

    private void LegsSound()
    {
        AudioManager.instance.PlaySoundEffect(stepSE, transform.position);
    }
}
