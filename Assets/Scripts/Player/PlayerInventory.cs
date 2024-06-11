using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    public static PlayerInventory instance;

    private int _keyCardCount = 0;

    public int keyCardCount
    {
        get { return _keyCardCount; }
        set
        {
            _keyCardCount = value;
            UpdateUI();
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
