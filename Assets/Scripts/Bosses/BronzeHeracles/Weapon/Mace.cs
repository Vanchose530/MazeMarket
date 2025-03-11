using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MaceAttackState
{
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();

        if (obj != null)
        {
            obj.TakeDamage(maceDamage, transform);

            if (collision.gameObject.CompareTag("Player"))
                AudioManager.instance.PlaySoundEffect(hitSE, transform.position);
        }
        
    }
}
