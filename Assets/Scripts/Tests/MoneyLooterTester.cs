using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyLooterTester : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage, Transform attack = null)
    {
        Destroy(this.gameObject);
    }
}
