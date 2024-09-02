using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGun : LootObject
{
    [Header("Loot Gun")]
    [SerializeField] private Gun gun;
    [SerializeField] private int ammoToAdd;

    public override bool GetCanLoot()
    {
        return !PlayerWeaponsManager.instance.IsGunSlotsFull();
    }

    public override void Loot()
    {
        PlayerWeaponsManager.instance.AddWeapon(gun);

        if (ammoToAdd > 0)
            PlayerWeaponsManager.instance.AddAmmoByType(gun.ammoType, ammoToAdd);
    }
}
