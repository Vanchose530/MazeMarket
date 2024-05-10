using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMiasma : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }
}
