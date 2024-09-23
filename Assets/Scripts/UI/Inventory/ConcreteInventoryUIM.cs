using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConcreteInventoryUIM : InventoryUIM
{
    [Header("Inventory Panel")]
    [SerializeField] private GameObject inventoryPanel;

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
        inventoryPanel.SetActive(false);
    }

    public override void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public override void SetAmmoByType(int count, AmmoTypes type)
    {
        switch (type)
        {
            case AmmoTypes.LightBullets:
                lightAmmoTMP.text = System.Convert.ToString(count);
                break;
            case AmmoTypes.MediumBullets:
                mediumAmmoTMP.text = System.Convert.ToString(count);
                break;
            case AmmoTypes.HeavyBullets:
                heavyAmmoTMP.text = System.Convert.ToString(count);
                break;
            case AmmoTypes.Shells:
                shellsTMP.text = System.Convert.ToString(count);
                break;
        }
    }

    public override void SetMoney(int count)
    {
        moneyTMP.text = System.Convert.ToString(count);
    }

    public override void SetVoidBottle(int count)
    {
        voidBottleTMP.text = System.Convert.ToString(count);
    }
}
