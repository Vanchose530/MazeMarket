using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIM : MonoBehaviour
{
    public static ShopUIM instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private List<ProductSlot> slots;
    public int slotsCount { get { return slots.Count; } }
    [SerializeField] private Sprite _voidSlotImage;
    public Sprite voidSlotImage {  get { return _voidSlotImage; } }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Shop UI Manager in scene!");
        instance = this;

        shopPanel.active = false;
    }

    public void ShowShopUI() => shopPanel.SetActive(true);
    public void HideShopUI() => shopPanel.SetActive(false);

    public void SetProductsInSlots(Product[] products)
    {
        for (int i = 0; i < products.Length; i++)
        {
            slots[i].SetProduct(products[i]);
        }
    }

    public void ClearSlots()
    {
        foreach (var slot in slots)
        {
            slot.SetVoid();
        }
    }
}
