using System;
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
    [SerializeField] private GunSlotUI gunSlotThree;
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
        GunSlotUI slotUI = null;

        switch (slot)
        {
            case 1:
                pos = gunSlotOne.transform.position;
                slotUI = gunSlotOne;
                break;
            case 2:
                pos = gunSlotTwo.transform.position;
                slotUI = gunSlotTwo;
                break;
            case 3:
                pos = gunSlotThree.transform.position;
                slotUI = gunSlotThree;
                break;
            case 4:
                pos = meleeWeaponSlot.transform.position;

                ammoInGunBuffer = -1;
                allAmmoBuffer = -1;
                magazineSizeBuffer = -1;
                break;
            default:
                Debug.LogWarning("Chosen missing UI slot: " + slot);
                pos = new Vector3(-1000, -1000, 0);
                break;
        }

        if (slotUI != null)
        {
            // Debug.Log("Slot UI not null");
            if (slotUI.gun != null)
            {
                // Debug.Log("Gun not null");
                SetAmmoInGun(slotUI.gun.ammoInMagazine, slotUI.gun.magazineSize);
                SetAllAmmo(Player.instance.weaponsManager.GetAmmoByType(slotUI.gun.ammoType),
                    slotUI.gun.ammoType);
            }
            else
            {
                StartCoroutine(SetAmmoCoroutine(slotUI));
            }
        }

        choosenWeaponMark.transform.position = pos;

        choosenWeaponIndex = slot;
    }

    // костыль необходимый, чтобы оружие успевалось подобраться и попасть в слот прежде чем выставлять патроны в ui
    IEnumerator SetAmmoCoroutine(GunSlotUI gunSlot)
    {
        for (int i = 0; i < 10;  i++)
        {
            try
            {
                SetAmmoInGun(gunSlot.gun.ammoInMagazine, gunSlot.gun.magazineSize);
                SetAllAmmo(Player.instance.weaponsManager.GetAmmoByType(gunSlot.gun.ammoType),
                    gunSlot.gun.ammoType);

                break;
            }
            catch (NullReferenceException)
            {
                // по сути тут должен быть yield return, но yield не может быть в блоке catch
            }
            yield return new WaitForSecondsRealtime(0.01f);

            if (i == 9)
                Debug.LogError("Cant set ammo in gun panel, because gun slot is void!");
        }
        yield return null;
    }

    public override void ShowAmmoPanel(int ammoInGun, int magazineSize, int allAmmoCount, AmmoTypes type)
    {
        SetAmmoInGun(ammoInGun, magazineSize);
        SetAllAmmo(allAmmoCount, type);
    }

    public override void SetAllAmmo(int count, AmmoTypes type)
    {
        // Debug.Log("Set all ammo");

        SetAmmoImage(type);

        allAmmoTMP.text = Convert.ToString(count);
    }

    public override void SetAllAmmo(int count)
    {
        // Debug.Log("Set all ammo");

        allAmmoTMP.text = Convert.ToString(count);
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

    public override void HideAmmoPanel()
    {
        // Debug.Log("Hide Ammo Panel");

        ammoInGunTMP.text = "";
        allAmmoTMP.text = "";

        if (reloadingCoroutine != null)
            StopCoroutine(reloadingCoroutine);

        ammoImage.enabled = false;
    }

    public override void SetAmmoInGun(int ammoInGun, int magazineSize)
    {
        Debug.Log("Set in gun");

        string text = ammoInGun.ToString() + "/" + magazineSize.ToString();
        ammoInGunTMP.text = text;

        allAmmoBuffer = ammoInGun;
        magazineSizeBuffer = magazineSize;
    }

    public override void SetAmmoInGun(int ammoInGun)
    {
        Debug.Log("Set in gun");

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

    public override void SetGunToSlotThree(Gun gun)
    {
        if (gun != null)
            gunSlotThree.SetGun(gun);
        else
            gunSlotThree.Clear();
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

    public override void StopReloading()
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
