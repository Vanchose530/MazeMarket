using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeaponsManager : MonoBehaviour, IDataPersistence
{
    public Gun firstSlotGun { get; set; }
    public Gun secondSlotGun { get; set; }
    public Gun thirdSlotGun { get; set; }
    public MeleeWeapon slotMeleeWeapon { get; set; }

    public Weapon currentWeapon { get; private set; }
    public Gun currentGun { get; private set; }
    public MeleeWeapon currentMeleeWeapon { get; private set; }

    [SerializeField] private float dropDistance = 2.0f;
    [SerializeField] private float dropDelayTimer = 2f;
    private bool isDropDelay = false;

    private int weaponInventoryId = 0;

    private IEnumerator reloadingCoroutine;

    const string PATH_TO_WEAPON_PREFABS = "Items\\Weapons\\";

    int lightBullets;
    int mediumBullets;
    int heavyBullets;
    int shells;

    public bool reloadingProccess
    {
        get
        {
            if (currentGun != null)
            {
                return currentGun.reloading;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Cant drop weapon")]
    [SerializeField] private LayerMask cantDropWeaponLayer;

    [Header("Setup")]
    public Player player;

    public float currentWeaponCooldown { get; private set; }

    private IEnumerator Start()
    {
        SetGunOrMelee();

        yield return new WaitForSeconds(0.1f);

        UpdateWeaponsUI();

        MainUIM.instance.inventory.SetAmmoByType(GetAmmoByType(AmmoTypes.LightBullets), AmmoTypes.LightBullets);
        MainUIM.instance.inventory.SetAmmoByType(GetAmmoByType(AmmoTypes.MediumBullets), AmmoTypes.MediumBullets);
        MainUIM.instance.inventory.SetAmmoByType(GetAmmoByType(AmmoTypes.HeavyBullets), AmmoTypes.HeavyBullets);
        MainUIM.instance.inventory.SetAmmoByType(GetAmmoByType(AmmoTypes.Shells), AmmoTypes.Shells);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.input.onRemoveWeaponPressed += RemoveWeapon;
        GameEventsManager.instance.input.onChangeWeaponPressed += ChangeWeapon;
        GameEventsManager.instance.playerWeapons.onWeaponChanged += CheckAmmo;
        GameEventsManager.instance.playerWeapons.onWeaponChanged += UpdateForWeaponChoosenUI;

        GameEventsManager.instance.input.onFirstWeaponChoosen += ToFirstWeapon;
        GameEventsManager.instance.input.onSecondWeaponChoosen += ToSecondWeapon;
        GameEventsManager.instance.input.onThirdWeaponChoosen += ToThirdWeapon;
        GameEventsManager.instance.input.onMeleeWeaponChoosen += ToMeleeWeapon;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.input.onRemoveWeaponPressed -= RemoveWeapon;
        GameEventsManager.instance.input.onChangeWeaponPressed -= ChangeWeapon;
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= CheckAmmo;
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= UpdateForWeaponChoosenUI;

        GameEventsManager.instance.input.onFirstWeaponChoosen -= ToFirstWeapon;
        GameEventsManager.instance.input.onSecondWeaponChoosen -= ToSecondWeapon;
        GameEventsManager.instance.input.onThirdWeaponChoosen -= ToThirdWeapon;
        GameEventsManager.instance.input.onMeleeWeaponChoosen -= ToMeleeWeapon;
    }

    private void Update()
    {
        CountTimeVariables();
        UpdateUI(); // затратно делать каждый кадр
    }

    public void LoadData(GameData data)
    {
        // this.weapons.Clear();
        foreach (KeyValuePair<string, int> pair in data.playerWeapons)
        {
            if (pair.Value != -1)
            {
                Gun gun = Instantiate(GetWeaponByName(pair.Key)) as Gun;
                gun.ammoInMagazine = pair.Value;
                // weapons.Add(gun);
            }
            else
            {
                Weapon weapon = Instantiate(GetWeaponByName(pair.Key));
                // weapons.Add(weapon); 
            }
        }

        this.lightBullets = data.lightBulletsCount;
        this.mediumBullets = data.mediumBulletsCount;
        this.heavyBullets = data.heavyBulletsCount;
        this.shells = data.shellsCount;
    }

    public void SaveData(ref GameData data)
    {
        data.playerWeapons.Clear();
        //foreach (Weapon weapon in this.weapons)
        //{
        //    try
        //    {
        //        Gun gun = (Gun)weapon;
        //        data.playerWeapons.Add(gun.displayName, gun.ammoInMagazine);
        //    }
        //    catch (System.InvalidCastException)
        //    {
        //        data.playerWeapons.Add(weapon.displayName, -1);
        //    }
        //}

        data.lightBulletsCount = this.lightBullets;
        data.mediumBulletsCount = this.mediumBullets;
        data.heavyBulletsCount = this.heavyBullets;
        data.shellsCount = this.shells;
    }
    
    public bool IsGunSlotsFull()
    {
        return firstSlotGun != null
            && secondSlotGun != null
            && thirdSlotGun != null;
    }

    public void AddWeapon(Weapon newWeapon)
    {
        if (newWeapon.GetType() == typeof(Gun))
        {
            
            if (firstSlotGun == null)
            {
                firstSlotGun = (Gun) newWeapon;
                ToFirstWeapon();
            }
            else if (secondSlotGun == null)
            {
                secondSlotGun = (Gun) newWeapon;
                ToSecondWeapon();
            }
            else if (thirdSlotGun == null)
            {
                thirdSlotGun = (Gun) newWeapon;
                ToThirdWeapon();
            }
            else
            {
                // —ообщение о том, что нет места дл€ огнестрела в инвентаре
            }
        }
        else if (newWeapon.GetType() == typeof(MeleeWeapon))
        {
            if (slotMeleeWeapon == null)
            {
                slotMeleeWeapon = (MeleeWeapon) newWeapon;
                ToMeleeWeapon();
            }
            else
            {
                // —ообщение о том, что нет места дл€ оружий ближнего бо€ в инвентаре
            }
        }
    }

    public int GetAmmoByType(AmmoTypes type)
    {
        switch (type)
        {
            case AmmoTypes.LightBullets:
                return lightBullets;
            case AmmoTypes.MediumBullets:
                return mediumBullets;
            case AmmoTypes.HeavyBullets:
                return heavyBullets;
            case AmmoTypes.Shells:
                return shells;
            default:
                return 0;
        }
    }

    public void AddAmmoByType(AmmoTypes type, int addValue)
    {
        switch (type)
        {
            case AmmoTypes.LightBullets:
                lightBullets += addValue;
                break;
            case AmmoTypes.MediumBullets:
                mediumBullets += addValue;
                break;
            case AmmoTypes.HeavyBullets:
                heavyBullets += addValue;
                break;
            case AmmoTypes.Shells:
                shells += addValue;
                break;
        }

        MainUIM.instance.inventory.SetAmmoByType(GetAmmoByType(type), type);
        // InventoryUIManager.instance.UpdateAmmoCounters();
    }

    public void SetAmmoByType(AmmoTypes type, int setValue)
    {
        switch (type)
        {
            case AmmoTypes.LightBullets:
                lightBullets = setValue;
                break;
            case AmmoTypes.MediumBullets:
                mediumBullets = setValue;
                break;
            case AmmoTypes.HeavyBullets:
                heavyBullets = setValue;
                break;
            case AmmoTypes.Shells:
                shells = setValue;
                break;
        }

        MainUIM.instance.inventory.SetAmmoByType(GetAmmoByType(type), type);
        // InventoryUIManager.instance.UpdateAmmoCounters();
    }

    public void ChangeWeapon()
    {
        if (firstSlotGun == null
            && secondSlotGun == null
            && firstSlotGun == null)
        {
            ToMeleeWeapon();
        }
        else
        {
            if (weaponInventoryId == 1)
            {
                if (secondSlotGun)
                    ToSecondWeapon();
                else if (thirdSlotGun)
                    ToThirdWeapon();
                else
                    ToMeleeWeapon();
            }
            else if (weaponInventoryId == 2)
            {
                if (thirdSlotGun)
                    ToThirdWeapon();
                else
                    ToMeleeWeapon();
            }
            else if (weaponInventoryId == 3)
            {
                ToMeleeWeapon();
            }
            else if (weaponInventoryId == 4)
            {
                if (firstSlotGun)
                    ToFirstWeapon();
                else if (secondSlotGun)
                    ToSecondWeapon();
                else if (thirdSlotGun)
                    ToThirdWeapon();
            }
        }
    }

    public void RemoveWeapon()
    {
        if (currentWeapon != null && Player.instance.CheckObstacles(dropDistance + 0.1f, cantDropWeaponLayer))
        {
            HintsManager.instance.ShowDefaultNotice("Ќе могу выбросить здесь", 3f);
            return;
        }

        if (isDropDelay)
            return;

        if (currentWeapon != null)
        {
            currentWeapon.onAttack -= SetCooldown;

            CreateDrop();

            Destroy(currentWeapon);

            currentWeapon = null;

            switch (weaponInventoryId)
            {
                case 1:
                    firstSlotGun = null;
                    break;
                case 2:
                    secondSlotGun = null;
                    break;
                case 3:
                    thirdSlotGun = null;
                    break;
                case 4:
                    slotMeleeWeapon = null;
                    break;
            }

            // ToMeleeWeapon();
            ChangeWeapon();

            StartCoroutine(DropDelay());
            UpdateWeaponsUI();
        }
    }

    private void CreateDrop()
    {
        Instantiate(Resources.Load<GameObject>(PATH_TO_WEAPON_PREFABS + currentWeapon.name.Replace("(Clone)", " ") + "Item"),
            player.transform.position + (Vector3)InputManager.instance.lookDirection * dropDistance,
            player.transform.rotation);
    }

    private IEnumerator DropDelay()
    {
        isDropDelay = true;
        yield return new WaitForSeconds(dropDelayTimer);
        isDropDelay = false;
    }

    private void SetGunOrMelee()
    {
        if (currentWeapon == null)
        {
            currentGun = null;
            currentMeleeWeapon = null;
        }
        else if (currentWeapon.GetType() == typeof(Gun))
        {
            currentGun = (Gun) currentWeapon;
            currentMeleeWeapon = null;
        }
        else if (currentWeapon.GetType() == typeof(MeleeWeapon))
        {
            currentMeleeWeapon = (MeleeWeapon) currentWeapon;
            currentGun = null;
        }
    }

    public void CheckAmmo()
    {
        if (currentGun != null && currentGun.ammoInMagazine == 0)
        {
            if (GetAmmoByType(currentGun.ammoType) == 0)
                HintsManager.instance.ShowWarningNotice(" ончились патроны!");
            else
                HintsManager.instance.ShowWarningNotice("Ќужна перезар€дка! (R)");
        }
        else
            HintsManager.instance.HideWarningNotice();
    }

    private Weapon GetWeaponByName(string name)
    {
        Weapon weapon = Instantiate(Resources.Load<Weapon>("Weapons/Guns/" + name)); // »—ѕ–ј¬»“№ ѕ”“№  ќ√ƒј ѕќя¬я“—я ќ–”∆»я ЅЋ»∆Ќ≈√ќ Ѕќя!
        return weapon;
    }

    // ћетоды дл€ перезар€дки оружиий
    public void Reload()
    {
        if (currentGun == null)
            return;

        if (GetAmmoByType(currentGun.ammoType) != 0
            && currentGun.magazineSize != currentGun.ammoInMagazine
            && !currentGun.reloading)
        {
            reloadingCoroutine = StartReloadig(currentGun);
            StartCoroutine(reloadingCoroutine);
        }
    }

    AudioSource reloadingSound; // используетс€ исключительно в корутине и при еЄ остановке!
    private IEnumerator StartReloadig(Gun gun)
    {
        gun.reloading = true;

        player.PlayReloadingAnimation();

        reloadingSound = AudioManager.instance.GetSoundEffectAS(gun.reloadingSE);

        Destroy(reloadingSound, gun.reloadTime);

        yield return new WaitForSeconds(gun.reloadTime);

        int magazineDelta = gun.magazineSize - gun.ammoInMagazine;

        if (magazineDelta >= GetAmmoByType(gun.ammoType))
        {
            gun.ammoInMagazine += GetAmmoByType(gun.ammoType);
            SetAmmoByType(gun.ammoType, 0);
        }
        else
        {
            gun.ammoInMagazine = gun.magazineSize;
            AddAmmoByType(gun.ammoType, -magazineDelta);
        }

        GameEventsManager.instance.playerWeapons.ReloadingEnd();

        CheckAmmo();

        gun.reloading = false;
    }

    private void StopGunReloading()
    {
        if (currentGun == null)
            return;

        Destroy(reloadingSound);

        currentGun.reloading = false;
        GameEventsManager.instance.playerWeapons.ReloadingEnd();

        if (reloadingCoroutine != null)
            StopCoroutine(reloadingCoroutine);
    }

    // ћетод дл€ установки кулдауна на оружие
    public void SetCooldown()
    {
        if (currentGun != null)
        {
            currentWeaponCooldown = 1 / currentGun.firingRate;
        }
        else if (currentMeleeWeapon != null)
        {
            currentWeaponCooldown = currentMeleeWeapon.attackCooldown;
        }
    }

    // ћетод дл€ счЄта временных переменных
    void CountTimeVariables()
    {
        if (currentWeaponCooldown > 0)
            currentWeaponCooldown -= Time.deltaTime;
    }

    // ћетоды дл€ смены оружий

    private void BeforeChangeWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.onAttack -= SetCooldown;
    }

    private void AfterChangeWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.onAttack += SetCooldown;

        StopGunReloading();
        SetGunOrMelee();
        GameEventsManager.instance.playerWeapons.WeaponChanged();
        UpdateWeaponsUI();
    }

    public void ToFirstWeapon()
    {
        if (firstSlotGun != null)
        {
            BeforeChangeWeapon();

            weaponInventoryId = 1;

            currentWeapon = firstSlotGun;

            AfterChangeWeapon();
        }
    }
    public void ToSecondWeapon()
    {
        if (secondSlotGun != null)
        {
            BeforeChangeWeapon();

            weaponInventoryId = 2;

            currentWeapon = secondSlotGun;

            AfterChangeWeapon();
        }
    }
    public void ToThirdWeapon()
    {
        if (thirdSlotGun != null)
        {
            BeforeChangeWeapon();

            weaponInventoryId = 3;

            currentWeapon = thirdSlotGun;

            AfterChangeWeapon();
        }
    }

    public void ToMeleeWeapon()
    {
        BeforeChangeWeapon();

        weaponInventoryId = 4;

        currentWeapon = slotMeleeWeapon;

        AfterChangeWeapon();
    }

    // ћетоды дл€ работы с UI

    void UpdateUI()
    {
        // Debug.Log("Current gun null: " + Convert.ToString(currentGun == null));
        // Debug.Log("Current weapon null: " + Convert.ToString(currentWeapon == null));
        if (currentGun != null)
        {
            MainUIM.instance.weapons.ShowAmmoPanel(currentGun.ammoInMagazine,
                currentGun.magazineSize, GetAmmoByType(currentGun.ammoType), currentGun.ammoType);
        }
        else
        {
            MainUIM.instance.weapons.HideAmmoPanel();
        }
    }

    private void UpdateWeaponsUI()
    {
        MainUIM.instance.weapons.SetGunToSlotOne(firstSlotGun);
        MainUIM.instance.weapons.SetGunToSlotTwo(secondSlotGun);
        MainUIM.instance.weapons.SetGunToSlotThree(thirdSlotGun);

        // когда будут милишки
        // MainUIM.instance.weapons.SetMeleeWeaponToSlot(meleeWeapon);

        MainUIM.instance.weapons.SetMeleeWeaponToSlot(null);

        UpdateForWeaponChoosenUI();
    }

    private void UpdateForWeaponChoosenUI()
    {
        if (currentGun != null)
        {
            MainUIM.instance.weapons.ChooseWeaponSlot(weaponInventoryId);
        }
        else
        {
            MainUIM.instance.weapons.ChooseWeaponSlot(4);
        }
    }
}
