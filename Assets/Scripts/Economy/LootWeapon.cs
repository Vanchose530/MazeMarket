
using UnityEngine;
[CreateAssetMenu(fileName = "New Loot Weapon", menuName = "Scriptable Objects/LootObjects", order = 1)]
public class LootWeapon : LootObject
{
    Weapon weapon;
    AmmoTypes ammoType;
    int ammoCount;

    public override bool GetCanLoot()
    {
        return !PlayerWeaponsManager.instance.IsGunSlotsFull();
    }
    public override void Loot()
    {
        PlayerWeaponsManager.instance.AddWeapon(weapon);
        PlayerWeaponsManager.instance.AddAmmoByType(ammoType, ammoCount);
    }
}
