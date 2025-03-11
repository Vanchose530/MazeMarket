using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RecoverAttack : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private int recoverDamage;
    [Header("Sound")]
    [SerializeField] private SoundEffect hitSE;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();

        if (obj != null)
        {
            obj.TakeDamage(recoverDamage, transform);

            if (collision.gameObject.CompareTag("Player"))
                AudioManager.instance.PlaySoundEffect(hitSE, transform.position);
        }

    }
}
