using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    public int maceDamage = 1;
    private void OnTriggerStay2D(Collider2D collision)
    { 
       IDamagable obj = collision.gameObject.GetComponent<IDamagable>();
       if (obj != null)
        obj.TakeDamage(maceDamage, transform);
    }
}
