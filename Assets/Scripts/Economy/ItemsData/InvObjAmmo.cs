
using UnityEngine;

public class InvObjAmmo : InventoryObject
{
    public AmmoTypes ammoType;
    public int ammoCount;
    public override void ToPlayerInventory()
    {
        PlayerWeaponsManager.instance.AddAmmoByType(ammoType, ammoCount);
    }
}
