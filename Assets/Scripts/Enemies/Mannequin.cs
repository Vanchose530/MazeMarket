using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour, IDamagable
{
    [Header("Setup")]
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private SoundEffect damageSE;

    public void TakeDamage(int damage, Transform attack = null)
    {
        AudioManager.instance.PlaySoundEffect(damageSE, 2f);

        if (attack != null)
        {
            var effect = Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), attack.rotation);
            Destroy(effect, 1f);
        }
    }
}
