using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Product", menuName = "Products/Gun Product", order = 1)]
public class GunProduct : Product
{
    [Header("Gun")]
    [SerializeField] private Gun gun;
    [SerializeField] private int ammoCount;

    override public bool GetCanBuy()
    {
        return !PlayerWeaponsManager.instance.IsGunSlotsFull();
    }

    public override void Buy()
    {
        Gun gunToBuy = Instantiate(gun);
        gunToBuy.ammoInMagazine = gun.magazineSize;
        PlayerWeaponsManager.instance.AddWeapon(gunToBuy);
        PlayerWeaponsManager.instance.AddAmmoByType(gun.ammoType, ammoCount);
    }
}
