using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConcreteWeaponsUIM : WeaponsUIM
{
    [Header("Weapon Slots")]
    [SerializeField] private GunSlotUI gunSlotOne;
    [SerializeField] private GunSlotUI gunSlotTwo;
    [SerializeField] private GunSlotUI gunSlotFree;
    [SerializeField] private MeleeWeaponSlotUI meleeWeaponSlot;
    [SerializeField] private GameObject choosenWeaponMark;
    int choosenWeaponIndex;

    [Header("Ammo in Gun")]
    [SerializeField] private TextMeshProUGUI ammoInGunTMP;
    [SerializeField] private TextMeshProUGUI allAmmoTMP;
    [SerializeField] private Image ammoImage;
    int ammoInGunBuffer;
    int magazineSizeBuffer;
    int allAmmoBuffer;

    [Header("Ammo Sprites")]
    [SerializeField] private Sprite lightAmmoSprite;
    [SerializeField] private Sprite mediumAmmoSprite;
    [SerializeField] private Sprite heavyAmmoSprite;
    [SerializeField] private Sprite shellsSprite;

    public override void ChooseWeaponSlot(int slot)
    {
        Vector3 pos;

        switch (slot)
        {
            case 1:
                pos = gunSlotOne.transform.position;

                SetAmmoInGun(gunSlotOne.gun.ammoInMagazine, gunSlotOne.gun.magazineSize);
                SetAllAmmo(PlayerWeaponsManager.instance.GetAmmoByType(gunSlotOne.gun.ammoType),
                    gunSlotOne.gun.ammoType);
                break;
            case 2:
                pos = gunSlotTwo.transform.position;

                SetAmmoInGun(gunSlotTwo.gun.ammoInMagazine, gunSlotTwo.gun.magazineSize);
                SetAllAmmo(PlayerWeaponsManager.instance.GetAmmoByType(gunSlotTwo.gun.ammoType),
                    gunSlotTwo.gun.ammoType);
                break;
            case 3:
                pos = gunSlotFree.transform.position;

                SetAmmoInGun(gunSlotFree.gun.ammoInMagazine, gunSlotFree.gun.magazineSize);
                SetAllAmmo(PlayerWeaponsManager.instance.GetAmmoByType(gunSlotFree.gun.ammoType),
                    gunSlotFree.gun.ammoType);
                break;
            case 4:
                pos = meleeWeaponSlot.transform.position;
                ammoInGunBuffer = -1;
                allAmmoBuffer = -1;
                magazineSizeBuffer = -1;
                break;
            default:
                pos = new Vector3(-1000, -1000, 0);
                break;
        }

        choosenWeaponIndex = slot;
    }

    public override void SetAllAmmo(int count, AmmoTypes type)
    {
        SetAmmoImage(type);

        allAmmoTMP.text = System.Convert.ToString(count);
    }

    public override void SetAllAmmo(int count)
    {
        allAmmoTMP.text = System.Convert.ToString(count);
    }

    private void SetAmmoImage(AmmoTypes type)
    {
        ammoImage.enabled = true;

        switch (type)
        {
            case AmmoTypes.LightBullets:
                ammoImage.sprite = lightAmmoSprite;
                break;
            case AmmoTypes.MediumBullets:
                ammoImage.sprite = mediumAmmoSprite;
                break;
            case AmmoTypes.HeavyBullets:
                ammoImage.sprite = heavyAmmoSprite;
                break;
            case AmmoTypes.Shells:
                ammoImage.sprite = shellsSprite;
                break;
        }
    }

    private void DisableAmmoImage()
    {
        ammoImage.enabled = false;
    }

    public override void SetAmmoInGun(int ammoInGun, int magazineSize)
    {
        string text = ammoInGun.ToString() + "/" + magazineSize.ToString();
        ammoInGunTMP.text = text;

        allAmmoBuffer = ammoInGun;
        magazineSizeBuffer = magazineSize;
    }

    public override void SetAmmoInGun(int ammoInGun)
    {
        string magazineSize = ammoInGunTMP.text.Split('/')[1];
        string text = ammoInGun.ToString() + "/" + magazineSize;
        ammoInGunTMP.text = text;

        allAmmoBuffer = ammoInGun;
    }

    public override void SetGunToSlotOne(Gun gun)
    {
        if (gun != null)
            gunSlotOne.SetGun(gun);
        else
            gunSlotOne.Clear();
    }

    public override void SetGunToSlotTwo(Gun gun)
    {
        if (gun != null)
            gunSlotTwo.SetGun(gun);
        else
            gunSlotTwo.Clear();
    }

    public override void SetGunToSlotFree(Gun gun)
    {
        if (gun != null)
            gunSlotFree.SetGun(gun);
        else
            gunSlotFree.Clear();
    }

    public override void SetMeleeWeaponToSlot(MeleeWeapon meleeWeapon)
    {
        if (meleeWeapon != null)
            meleeWeaponSlot.SetMeleeWeapon(meleeWeapon);
        else
            meleeWeaponSlot.Clear();
    }

    Coroutine reloadingCoroutine;

    public override void SetReloading(int time)
    {
        reloadingCoroutine = StartCoroutine(ReloadingCoroutine(time));
    }

    private void StopReloading()
    {
        if (reloadingCoroutine != null)
            StopCoroutine(reloadingCoroutine);

        if (choosenWeaponIndex < 4)
        {
            SetAllAmmo(allAmmoBuffer);
            SetAmmoInGun(ammoInGunBuffer, magazineSizeBuffer);
        }
    }

    IEnumerator ReloadingCoroutine(int time)
    {
        allAmmoTMP.text = "";
        ammoInGunTMP.text = "Перезарядка";
        yield return new WaitForSeconds(time);
    }
}
