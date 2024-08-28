using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static ShopManager _instance;
    public static ShopManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<ShopManager>();
                _instance.name = _instance.GetType().ToString();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public bool onShoping { get; private set; }

    public void StartShoping(Product[] products)
    {
        onShoping = true;
        PlayerConditionsManager.instance.currentCondition = PlayerConditions.Shoping;
        ShopUIM.instance.SetProductsInSlots(products);
        ShopUIM.instance.ShowShopUI();
        ShopUIM.instance.SelectFirstChoice();

        GameEventsManager.instance.input.onReloadPressed += EndShoping;
    }

    public void EndShoping()
    {
        onShoping = false;
        PlayerConditionsManager.instance.SetGamingCondition();
        ShopUIM.instance.ClearSlots();
        ShopUIM.instance.HideShopUI();

        GameEventsManager.instance.input.onReloadPressed -= EndShoping;
    }
}
