using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueLegs : MonoBehaviour
{
    [SerializeField] private Statue statue;
    private void LegsSound()
    {
        AudioManager.instance.PlaySoundEffect(statue.legsSE, statue.rb.position);
    }
}
