using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIM : MonoBehaviour
{
    public static ShopUIM instance { get; private set; }

    [SerializeField] private List<ProductSlot> slots;
    [SerializeField] private Sprite _voidSlotImage;
    public Sprite voidSlotImage {  get { return _voidSlotImage; } }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Shop UI Manager in scene!");
        instance = this;
    }
}
