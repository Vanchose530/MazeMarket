using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    public static PlayerInventory instance;

    [Header("Bottle")]
    [SerializeField] private int startCountGrenadeBottle = 0;
    [SerializeField] private int startCountHealthBottle = 0;
    [SerializeField] private int startCountEmptyBottle = 0;

    private int _keyCardCount;

    private int _countGrenadeBottle;
    private int _countHealthBottle;
    private int _countEmptyBottle;

    private int _money;

    public int keyCardCount
    {
        get { return _keyCardCount; }
        set
        {
            _keyCardCount = value;
            UpdateUI();
        }
    }
    public int countGrenadeBottle
    {
        get { return _countGrenadeBottle; }
        set
        {
            _countGrenadeBottle = value;
            UpdateUIGrenadeBottle();
        }
    }
    public int countHealthBottle
    {
        get { return _countHealthBottle; }
        set
        {
            _countHealthBottle = value;
            UpdateUIHealthBottle();
        }
    }
    public int countEmptyBottle
    {
        get { return _countEmptyBottle; }
        set
        {
            _countEmptyBottle = value;
            UpdateUIEmptyBottle();
        }
    }

    public int money
    {
        get { return _money; }
        set
        {
            _money = value;
            InventoryUIManager.instance.SetMoney(value);
        }
    }
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Player Inventory in scene");
        instance = this;
    }
    private void Start()
    {
        countGrenadeBottle = startCountGrenadeBottle;
        countHealthBottle = startCountHealthBottle;
        countEmptyBottle = startCountEmptyBottle;
    }

    private void UpdateUI()
    {
        if (keyCardCount == 0)
        {
            InventoryUIManager.instance.keyCardUI.SetActive(false);
        }
        else if (keyCardCount == 1)
        {
            InventoryUIManager.instance.keyCardUI.SetActive(true);
            InventoryUIManager.instance.keyCardCountText = "";
        }
        else
        {
            InventoryUIManager.instance.keyCardUI.SetActive(true);
            InventoryUIManager.instance.keyCardCountText = Convert.ToString(keyCardCount);
        }
    }
    private void UpdateUIGrenadeBottle() 
    {
        InventoryUIManager.instance.grenadeBottleCountText = Convert.ToString(countGrenadeBottle);
    }
    private void UpdateUIHealthBottle()
    {
        InventoryUIManager.instance.healthBottleCountText = Convert.ToString(countHealthBottle);
    }
    private void UpdateUIEmptyBottle()
    {
        InventoryUIManager.instance.emptyBottleCountText = Convert.ToString(countEmptyBottle);
    }

    public void LoadData(GameData data)
    {
        this.keyCardCount = data.keyCardCount;
    }

    public void SaveData(ref GameData data)
    {
        data.keyCardCount = this.keyCardCount;
    }
}
