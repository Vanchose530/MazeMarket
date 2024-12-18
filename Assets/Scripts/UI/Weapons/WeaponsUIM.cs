using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponsUIM : MonoBehaviour
{
    public abstract void SetGunToSlotOne(Gun gun);
    public abstract void SetGunToSlotTwo(Gun gun);
    public abstract void SetGunToSlotThree(Gun gun);
    public abstract void SetMeleeWeaponToSlot(MeleeWeapon meleeWeapon);
    public abstract void ChooseWeaponSlot(int slot);

    // ---------------------------------------------------

    public abstract void ShowAmmoPanel(int ammoInGun, int magazineSize, int allAmmoCount, AmmoTypes type);
    public abstract void SetAmmoInGun(int ammoInGun, int magazineSize);
    public abstract void SetAmmoInGun(int ammoInGun);
    public abstract void SetAllAmmo(int count, AmmoTypes type);
    public abstract void SetAllAmmo(int count);
    public abstract void HideAmmoPanel();

    // ---------------------------------------------------

    public abstract void SetReloading(int time);
    public abstract void StopReloading();
}
