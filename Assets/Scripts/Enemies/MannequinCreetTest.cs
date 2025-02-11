using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinCreetTest : MonoBehaviour, IDamagable
{
    [Header("Creet Test")]
    [SerializeField] private SoundEffect creetSE;
    [Range(0f, 100f)]
    [SerializeField] private int creetChance;
    [SerializeField] private int creetDamage;

    [Header("Setup")]
    [SerializeField] private SoundEffect damageSE;
    [SerializeField] private DamageParticles damageParticles;

    public void TakeDamage(int damage, Transform attack = null)
    {
        AudioManager.instance.PlaySoundEffect(damageSE, 2f);

        int r = Random.Range(0, 100);

        if (r < creetChance)
        {
            damage = creetDamage;
            AudioManager.instance.PlaySoundEffect(creetSE, 2f);
        }

        if (attack != null)
        {
            damageParticles.Play(damage, attack);
        }
    }
}
