using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager instance { get; private set; }

    [Header("General")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Weapons")]
    [SerializeField] private WeaponSlot[] weaponSlots;

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI lightBulletsCountTMP;
    [SerializeField] private TextMeshProUGUI mediumBulletsCountTMP;
    [SerializeField] private TextMeshProUGUI heavyBulletsCountTMP;
    [SerializeField] private TextMeshProUGUI shellsCountTMP;

    [Header("Key Cards")]
    [SerializeField] private GameObject _keyCardUI;
    public GameObject keyCardUI { get { return _keyCardUI; } }
    [SerializeField] private TextMeshProUGUI keyCardCountTMP;

    [Header("Bottle")]
    [SerializeField] private TextMeshProUGUI grenadeBottleCountTMP;
    [SerializeField] private TextMeshProUGUI healthBottleCountTMP;
    [SerializeField] private TextMeshProUGUI emptyBottleCountTMP;

    public string keyCardCountText
    {
        get { return keyCardCountTMP.text; }
        set { keyCardCountTMP.text = value; }
    }
    public string grenadeBottleCountText
    {
        get { return grenadeBottleCountTMP.text; }
        set { grenadeBottleCountTMP.text = value; }
    }
    public string healthBottleCountText
    {
        get { return healthBottleCountTMP.text; }
        set { healthBottleCountTMP.text = value; }
    }
    public string emptyBottleCountText
    {
        get { return emptyBottleCountTMP.text; }
        set { emptyBottleCountTMP.text = value; }
    }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Inventory UI script in scene");
        instance = this;
    }

    private void OnEnable()
    {
        Invoke("OnEnableAfterTime", 0.1f);
    }

    private void OnEnableAfterTime()
    {
        GameEventsManager.instance.input.onInventoryPerformed += ShowInventory;
        GameEventsManager.instance.input.onInventoryCanceled += HideInventory;
        GameEventsManager.instance.playerWeapons.onWeaponChanged += UpdateForWeaponChosen;

        UpdateAmmoCounters();
        UpdateWeaponSlots();

    }

    private void OnDisable()
    {
        GameEventsManager.instance.input.onInventoryPerformed -= ShowInventory;
        GameEventsManager.instance.input.onInventoryCanceled -= HideInventory;
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= UpdateForWeaponChosen;
    }

    private void Start()
    {
        HideInventory();
    }


    public void UpdateAmmoCounters()
    {
        lightBulletsCountTMP.text = System.Convert.ToString(PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.LightBullets));
        mediumBulletsCountTMP.text = System.Convert.ToString(PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.MediumBullets));
        heavyBulletsCountTMP.text = System.Convert.ToString(PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.HeavyBullets));
        shellsCountTMP.text = System.Convert.ToString(PlayerWeaponsManager.instance.GetAmmoByType(AmmoTypes.Shells));
    }

    public void UpdateWeaponSlots()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            try
            {
                weaponSlots[i].weapon = PlayerWeaponsManager.instance.weapons[i];
            }
            catch (System.ArgumentOutOfRangeException)
            {
                weaponSlots[i].weapon = null;
            }
        }
    }

    public void UpdateForWeaponChosen() 
    {
        foreach(var weapSlot in weaponSlots)
        {
            weapSlot.CheckForChosen();
        }
    }


    private void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    private void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
