using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeaponsManager : MonoBehaviour, IDataPersistence
{
    public static PlayerWeaponsManager instance { get; private set; }

    public List<Weapon> weapons { get; /*private*/ set; } // чтобы игрок мог переносить информацию между сценами private модификатор закоментирован

    public Weapon currentWeapon { get; private set; }
    public Gun currentGun { get; private set; }
    public MeleeWeapon currentMeleeWeapon { get; private set; }

    [SerializeField] private int weaponInventorySize;

    [SerializeField] private float dropDistance = 2.0f;
    [SerializeField] private float dropDelayTimer = 2f;
    private bool isDropDelay = false;

    private int weaponInventoryId = 0;

    private IEnumerator reloadingCoroutine;

    const string PATH_TO_WEAPON_PREFABS = "Items\\Weapons\\";
    /*
    [Header("Start Items")]
    public List<Weapon> startWeapons;
    public int startLightBullets;
    public int startMediumBullets;
    public int startHeavyBullets;
    public int startShells;
    */
    int lightBullets;
    int mediumBullets;
    int heavyBullets;
    int shells;

    [Header("Cant drop weapon")]
    [SerializeField] private LayerMask cantDropWeaponLayer;

    public float currentWeaponCooldown { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            UnityEngine.Debug.LogWarning("Find more than one Player Weapon Manager in scene");
        }
        instance = this;

        weapons = new List<Weapon>();
    }

    private IEnumerator Start()
    {
        //lightBullets = startLightBullets;
        //mediumBullets = startMediumBullets;
        //heavyBullets = startHeavyBullets;
        //shells = startShells;

        SetGunOrMelee();

        //if (startWeapons != null)
        //{
        //    foreach (Weapon weapon in startWeapons)
        //    {
        //        var w = Instantiate(weapon);

        //        try
        //        {
        //            Gun g = (Gun)w;

        //            g.ammoInMagazine = g.magazineSize;
        //        }
        //        catch (System.InvalidCastException) { }
                
        //        weapons.Add(w);
        //    }
        //}

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

        GameEventsManager.instance.input.onFirstWeaponChoosen += toFirstWeapon;
        GameEventsManager.instance.input.onSecondWeaponChoosen += toSecondWeapon;
        GameEventsManager.instance.input.onThirdWeaponChoosen += toThirdWeapon;
        GameEventsManager.instance.input.onMeleeWeaponChoosen += toMeleeWeapon;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.input.onRemoveWeaponPressed -= RemoveWeapon;
        GameEventsManager.instance.input.onChangeWeaponPressed -= ChangeWeapon;
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= CheckAmmo;
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= UpdateForWeaponChoosenUI;

        GameEventsManager.instance.input.onFirstWeaponChoosen -= toFirstWeapon;
        GameEventsManager.instance.input.onSecondWeaponChoosen -= toSecondWeapon;
        GameEventsManager.instance.input.onThirdWeaponChoosen -= toThirdWeapon;
        GameEventsManager.instance.input.onMeleeWeaponChoosen -= toMeleeWeapon;
    }

    private void Update()
    {
        // CheckAmmo(); // затратно делать каждый кадр
        UpdateUI(); // затратно делать каждый кадр
        CountTimeVariables();
    }

    public void LoadData(GameData data)
    {
        this.weapons.Clear();
        foreach (KeyValuePair<string, int> pair in data.playerWeapons)
        {
            if (pair.Value != -1)
            {
                Gun gun = Instantiate(GetWeaponByName(pair.Key)) as Gun;
                gun.ammoInMagazine = pair.Value;
                weapons.Add(gun);
            }
            else
            {
                Weapon weapon = Instantiate(GetWeaponByName(pair.Key));
                weapons.Add(weapon); 
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
        foreach (Weapon weapon in this.weapons)
        {
            try
            {
                Gun gun = (Gun)weapon;
                data.playerWeapons.Add(gun.displayName, gun.ammoInMagazine);
            }
            catch (System.InvalidCastException)
            {
                data.playerWeapons.Add(weapon.displayName, -1);
            }
        }

        data.lightBulletsCount = this.lightBullets;
        data.mediumBulletsCount = this.mediumBullets;
        data.heavyBulletsCount = this.heavyBullets;
        data.shellsCount = this.shells;
    }
    
    // проверка инвентар€ на "полность" дл€ подбора предметов
    public bool IsGunSlotsFull()
    {
        if (weaponInventoryId > weapons.Count && currentWeapon != null)
            return true;
        if (weapons.Count < weaponInventorySize)
            return false;
        if (weapons.Count == weaponInventorySize)
        {
            SwitchToNullWeapon();
            if (currentWeapon == null) return false;
        }
        return true;
    }

    private void SwitchToNullWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] == null)
            {
                currentWeapon = weapons[i];
                weaponInventoryId = i;
                return;
            }
        }
    }

    public void AddWeapon(Weapon newWeapon)
    {
        if (weapons.Count == weaponInventorySize)
        {
            try
            { 
                if (currentWeapon.displayName != newWeapon.displayName)
                    weapons[weaponInventoryId] = newWeapon;
            }
            catch(NullReferenceException ignored) 
            {
                if (currentWeapon == null) weapons[weaponInventoryId] = newWeapon;
            }
        }
        else
            weapons.Add(newWeapon);
            weaponInventoryId = weapons.Count - 1;

        if (currentWeapon != null)
            currentWeapon.onAttack -= SetCooldown;

        currentWeapon = weapons[weaponInventoryId];
        StopGunReloading();
        SetGunOrMelee();

        currentWeapon.onAttack += SetCooldown;

        GameEventsManager.instance.playerWeapons.WeaponChanged();
        UpdateWeaponsUI();
        // InventoryUIManager.instance.UpdateWeaponSlots();
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
        if (weapons.Count == 0)
        {
            toMeleeWeapon();
        }
        else
        {
            UnityEngine.Debug.Log("changed weapon by F");
            if (currentWeapon != null)
                currentWeapon.onAttack -= SetCooldown;

            try
            {
                currentWeapon = weapons[weaponInventoryId + 1];
                weaponInventoryId++;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                weaponInventoryId = 0;
                currentWeapon = weapons[weaponInventoryId];
            }

            currentWeapon.onAttack += SetCooldown;

            StopGunReloading();
            SetGunOrMelee();
        }

        GameEventsManager.instance.playerWeapons.WeaponChanged();
    }
    /* "Ќормальна€" попытка реализации смены оружи€.... в идеале перейти на это.
    public void ChangeWeapon(int _weaponInventoryId)
    {
        if (currentWeapon != null)
            currentWeapon.onAttack -= SetCooldown;

        currentWeapon = weapons[_weaponInventoryId];

        currentWeapon.onAttack += SetCooldown;

        StopGunReloading();
        SetGunOrMelee();

        GameEventsManager.instance.playerWeapons.WeaponChanged();
    }
    */

    public void RemoveWeapon()
    {
        UnityEngine.Debug.Log("logged T");

        if (Player.instance.CheckObstacles(dropDistance + 0.1f, cantDropWeaponLayer))
        {
            UnityEngine.Debug.Log("Cant drop it here");
            HintsManager.instance.ShowDefaultNotice("Ќе могу выбросить здесь", 3f);
            return;
        }

        if (isDropDelay)
            return;

        if (currentWeapon != null)
        {
            currentWeapon.onAttack -= SetCooldown;

            CreateDrop();

            weapons.Remove(currentWeapon);
            Destroy(currentWeapon);

            currentWeapon = null;
            
            StopGunReloading();
            SetGunOrMelee();


            GameEventsManager.instance.playerWeapons.WeaponChanged();
            
        }
        StartCoroutine(DropDelay());
        UpdateWeaponsUI();
        // InventoryUIManager.instance.UpdateWeaponSlots();
    }

    private void CreateDrop()
    {
        UnityEngine.Debug.Log("weapon in id " + weaponInventoryId);
        UnityEngine.Debug.Log(currentWeapon.name);
        UnityEngine.Debug.Log(PATH_TO_WEAPON_PREFABS + currentWeapon.name.Replace("(Clone)", " ") + "Item");
        Instantiate(Resources.Load<GameObject>(PATH_TO_WEAPON_PREFABS + currentWeapon.name.Replace("(Clone)", " ") + "Item"), Player.instance.transform.position + (Vector3)InputManager.instance.lookDirection * dropDistance, Player.instance.transform.rotation);
    }

    private IEnumerator DropDelay()
    {
        isDropDelay = true;
        yield return new WaitForSeconds(dropDelayTimer);
        isDropDelay = false;
    }

    private void SetGunOrMelee()
    {
        try { currentGun = (Gun)currentWeapon; }
        catch (System.InvalidCastException) { currentGun = null; }
        try { currentMeleeWeapon = (MeleeWeapon)currentWeapon; } catch (System.InvalidCastException) { currentMeleeWeapon = null; }

        //if (currentGun != null)
        //    AmmoUIManager.instance.ammoType = currentGun.ammoType; // в будущем обновл€ть ui при вызове событи€
    }

    public void CheckAmmo()
    {
        if (currentGun != null && currentGun.ammoInMagazine == 0 && GetAmmoByType(currentGun.ammoType) == 0)
            HintsManager.instance.ShowWarningNotice(" ончились патроны!");
        else
            HintsManager.instance.HideWarningNotice();
    }

    public void Reload()
    {
        if (currentGun == null)
            return;
        if (PlayerWeaponsManager.instance.GetAmmoByType(currentGun.ammoType) != 0 && currentGun.magazineSize != currentGun.ammoInMagazine && !currentGun.reloading)
        {
            reloadingCoroutine = StartReloadig(currentGun);
            StartCoroutine(reloadingCoroutine);
        }
    }

    private Weapon GetWeaponByName(string name)
    {
        Weapon weapon = Instantiate(Resources.Load<Weapon>("Weapons/Guns/" + name)); // »—ѕ–ј¬»“№ ѕ”“№  ќ√ƒј ѕќя¬я“—я ќ–”∆»я ЅЋ»∆Ќ≈√ќ Ѕќя!
        return weapon;
    }

    AudioSource reloadingSound; // используетс€ исключительно в корутине и при еЄ остановке!
    private IEnumerator StartReloadig(Gun gun)
    {
        gun.reloading = true;

        Player.instance.PlayReloadingAnimation();

        reloadingSound = AudioManager.instance.GetSoundEffectAS(gun.reloadingSE);

        Destroy(reloadingSound, gun.reloadTime);

        yield return new WaitForSeconds(gun.reloadTime);

        int magazineDelta = gun.magazineSize - gun.ammoInMagazine;

        if (magazineDelta >= PlayerWeaponsManager.instance.GetAmmoByType(gun.ammoType))
        {
            gun.ammoInMagazine += PlayerWeaponsManager.instance.GetAmmoByType(gun.ammoType);
            PlayerWeaponsManager.instance.SetAmmoByType(gun.ammoType, 0);
        }
        else
        {
            gun.ammoInMagazine = gun.magazineSize;
            PlayerWeaponsManager.instance.AddAmmoByType(gun.ammoType, -magazineDelta);
        }

        gun.reloading = false;
    }

    private void StopGunReloading()
    {
        if (currentGun == null)
            return;

        Destroy(reloadingSound);

        currentGun.reloading = false;
        if (reloadingCoroutine != null) StopCoroutine(reloadingCoroutine);
    }

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

    void UpdateUI()
    {
        if(currentGun != null)
        {
            //AmmoUIManager.instance.ammoPanelActive = true;
            //AmmoUIManager.instance.allAmmoText = Convert.ToString(GetAmmoByType(currentGun.ammoType));
            //AmmoUIManager.instance.ammoInGunText = Convert.ToString(currentGun.magazineSize) + "/" + Convert.ToString(currentGun.ammoInMagazine);

            //AmmoUIManager.instance.ammoPanelAnimator.SetBool("Reloading", currentGun.reloading);

            MainUIM.instance.weapons.ShowAmmoPanel(currentGun.ammoInMagazine,
                currentGun.magazineSize, GetAmmoByType(currentGun.ammoType), currentGun.ammoType);
        }
        else
        {
            // AmmoUIManager.instance.ammoPanelActive = false;
            MainUIM.instance.weapons.HideAmmoPanel();
        }
    }

    void CountTimeVariables()
    {
        if (currentWeaponCooldown > 0)
            currentWeaponCooldown -= Time.deltaTime;
    }

    // красиво будет если помен€ем на универсальный метод, а не разные дл€ каждой кнопки.
    public void toFirstWeapon()
    {
        if (weapons.Count >= 1)
        {
            if(currentWeapon != null)
                currentWeapon.onAttack -= SetCooldown;

            weaponInventoryId = 0;

            currentWeapon = weapons[weaponInventoryId];
            if(currentWeapon != null) currentWeapon.onAttack += SetCooldown;
            StopGunReloading();
            SetGunOrMelee();
            GameEventsManager.instance.playerWeapons.WeaponChanged();
            UpdateWeaponsUI();
            // InventoryUIManager.instance.UpdateWeaponSlots();

            // CheckAmmo();
        }
        else toMeleeWeapon();
    }
    public void toSecondWeapon()
    {
        if (weapons.Count >= 2)
        {
            if (currentWeapon != null)
                currentWeapon.onAttack -= SetCooldown;

            weaponInventoryId = 1;

            currentWeapon = weapons[weaponInventoryId];
            if (currentWeapon != null) currentWeapon.onAttack += SetCooldown;
            StopGunReloading();
            SetGunOrMelee();
            GameEventsManager.instance.playerWeapons.WeaponChanged();
            UpdateWeaponsUI();
            // InventoryUIManager.instance.UpdateWeaponSlots();

            // CheckAmmo();
        }
        else toMeleeWeapon();
    }
    public void toThirdWeapon()
    {
        if (weapons.Count >= 3)
        {
            if (currentWeapon != null)
                currentWeapon.onAttack -= SetCooldown;

            weaponInventoryId = 2;

            currentWeapon = weapons[weaponInventoryId];
            if (currentWeapon != null) currentWeapon.onAttack += SetCooldown;
            StopGunReloading();
            SetGunOrMelee();
            GameEventsManager.instance.playerWeapons.WeaponChanged();
            UpdateWeaponsUI();
            // InventoryUIManager.instance.UpdateWeaponSlots();

            // CheckAmmo();
        }
        else toMeleeWeapon();
    }

   // toMeleeWeapon -> старый ремув веапон
    public void toMeleeWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.onAttack -= SetCooldown;
        UnityEngine.Debug.Log("logged 4");

        currentWeapon = null;
        weaponInventoryId = 100; // большое значение чтобы в этот "слот" пушек не брать

        StopGunReloading();
        SetGunOrMelee();

        GameEventsManager.instance.playerWeapons.WeaponChanged();

        // CheckAmmo();
    }

    // UI

    private void UpdateWeaponsUI()
    {
        try { MainUIM.instance.weapons.SetGunToSlotOne((Gun)weapons[0]); }
        catch (ArgumentOutOfRangeException) { MainUIM.instance.weapons.SetGunToSlotOne(null); }

        try { MainUIM.instance.weapons.SetGunToSlotTwo((Gun)weapons[1]); }
        catch (ArgumentOutOfRangeException) { MainUIM.instance.weapons.SetGunToSlotTwo(null); }

        try { MainUIM.instance.weapons.SetGunToSlotFree((Gun)weapons[2]); }
        catch (ArgumentOutOfRangeException) { MainUIM.instance.weapons.SetGunToSlotFree(null); }

        // когда будут милишки
        // MainUIM.instance.weapons.SetMeleeWeaponToSlot(meleeWeapon);
        MainUIM.instance.weapons.SetMeleeWeaponToSlot(null);

        UpdateForWeaponChoosenUI();
    }

    private void UpdateForWeaponChoosenUI()
    {
        if (currentGun != null)
        {
            MainUIM.instance.weapons.ChooseWeaponSlot(weaponInventoryId + 1);
        }
        else
        {
            MainUIM.instance.weapons.ChooseWeaponSlot(4);
        }
    }
}
