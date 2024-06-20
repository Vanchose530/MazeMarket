using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string ignoreTag;
    [SerializeField] private int damage;
    [SerializeField] private Goplit goplit;
    private void Awake()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ignoreTag))
            return;

        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();
        goplit.attack = false;
        if (obj != null)
            obj.TakeDamage(damage, transform);
    }
}
