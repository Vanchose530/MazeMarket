
using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance { get; private set; }

    private int _currentMoneyCount = PlayerInventory.instance.moneyCount;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Economy Manager script in scene");
        instance = this;
    }

    private bool isValidOffer(int cost)
    {
        if (cost > _currentMoneyCount) return false;
        return true;
    }

    private void PayCost(int cost) { _currentMoneyCount -= cost; }

    private void CreateProduct()
    {

    }

    public void Deal(int cost) // TODO: нужно как-то отмечать, что мы покупаем -> зависит реализация выдачи
    {
        if (!isValidOffer(cost))
            return;
        
        PayCost(cost);

        CreateProduct();
    }

}
