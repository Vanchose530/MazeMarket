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
    [SerializeField] private int startCountGrenadeBottle = 3;
    [SerializeField] private int startCountHealthBottle = 1;
    [SerializeField] private int startCountEmptyBottle = 1;

    private int _keyCardCount;
    private int _keyCardCount = 0;
    private int _countGrenadeBottle;
    private int _countHealthBottle;
    private int _countEmptyBottle;



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

    
    private int _moneyCount;
    public int moneyCount
    {
        get { return _moneyCount; }
        set
        {
            _moneyCount = value;
            Debug.Log("Set money to " + value);
            UpdateUI ();
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
        //this.moneyCount = data.moneyCount; need realize
    }

    public void SaveData(ref GameData data)
    {
        data.keyCardCount = this.keyCardCount;
        //data.moneyCount = this.moneyCount; need realize
    }
}
