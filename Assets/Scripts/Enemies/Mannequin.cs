using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour, IDamageable
{
    [Header("Setup")]
    [SerializeField] private SoundEffect damageSE;
    [SerializeField] private DamageParticles damageParticles;

    public void TakeDamage(int damage, Transform attack = null)
    {
        AudioManager.instance.PlaySoundEffect(damageSE, 2f);

        if (attack != null)
        {
            damageParticles.Play(damage, attack);
        }
    }
}
