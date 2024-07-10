using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee")]
    public float attackRange = 0.5f;
    public float attackCooldown = 2f;
    int meleeAttackSide = 1;

    public override void Attack(Transform attackPoint)
    {

    }

    public override void ToPlayerInventory()
    {
        
    }
}
