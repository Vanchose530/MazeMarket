using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Gun", menuName = "Scriptable Objects/Loot/Gun", order = 1)]
public class LootGun : LootObject
{
    [Header("Loot Gun")]
    [SerializeField] private Gun gun;
    [SerializeField] private int ammoToAdd;

    public override bool GetCanLoot()
    {
        return !Player.instance.weaponsManager.IsGunSlotsFull();
    }

    public override void Loot()
    {
        Gun gunToAdd = Instantiate(gun);
        gunToAdd.ammoInMagazine = gunToAdd.magazineSize;

        Player.instance.weaponsManager.AddWeapon(gunToAdd);

        if (ammoToAdd > 0)
            Player.instance.weaponsManager.AddAmmoByType(gun.ammoType, ammoToAdd);
    }

    public override string GetLootString()
    {
        return gun.name;
    }
}
