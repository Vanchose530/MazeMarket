using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Legs : MonoBehaviour
{
    [SerializeField] private Goplit goplit;
    private void LegsSound()
    {
        AudioManager.instance.PlaySoundEffect(goplit.legsSE, goplit.rb.position);
    }
}
