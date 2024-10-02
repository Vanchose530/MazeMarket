using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyLooterTester : MonoBehaviour, IDamagable
{
    public GameObject looter;

    public void TakeDamage(int damage, Transform attack = null)
    {
        var inScene = Instantiate(looter);
        inScene.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
