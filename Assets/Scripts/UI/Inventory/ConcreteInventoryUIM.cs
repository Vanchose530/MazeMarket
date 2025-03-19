using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConcreteInventoryUIM : InventoryUIM
{
    [Header("Inventory Panel")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventoryMark;

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI lightAmmoTMP;
    [SerializeField] private TextMeshProUGUI mediumAmmoTMP;
    [SerializeField] private TextMeshProUGUI heavyAmmoTMP;
    [SerializeField] private TextMeshProUGUI shellsTMP;

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI moneyTMP;

    [Header("Void Bottle")]
    [SerializeField] private TextMeshProUGUI voidBottleTMP;

    public override void CloseInventory()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        if (inventoryMark != null)
            inventoryMark.SetActive(true);
    }

    public override void OpenInventory()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(true);
        if (inventoryMark != null)
            inventoryMark.SetActive(false);
    }

    public override void SetAmmoByType(int count, AmmoTypes type)
    {
        switch (type)
        {
            case AmmoTypes.LightBullets:
                lightAmmoTMP.text = count.ToString();
                break;
            case AmmoTypes.MediumBullets:
                mediumAmmoTMP.text = count.ToString();
                break;
            case AmmoTypes.HeavyBullets:
                heavyAmmoTMP.text = count.ToString();
                break;
            case AmmoTypes.Shells:
                shellsTMP.text = count.ToString();
                break;
        }
    }

    public override void SetMoney(int count)
    {
        moneyTMP.text = count.ToString();
    }

    public override void SetVoidBottle(int count)
    {
        voidBottleTMP.text = count.ToString();
    }
}
