using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDoor : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage=10;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player") {
            collision.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }
}
