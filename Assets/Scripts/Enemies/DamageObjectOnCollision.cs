using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObjectOnCollision : MonoBehaviour
{
    const string OBJECT_TAG = "Object";

    [Header("Damage")]
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(OBJECT_TAG))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakeDamage(damage, transform);
            }
        }
    }
}
