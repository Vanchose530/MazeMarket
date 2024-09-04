using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductSlot : MonoBehaviour
{
    [Header("Product Settings")]
    [SerializeField] private Image productImage;
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI productPrice;
    [SerializeField] private TextMeshProUGUI productCount;
    [SerializeField] private GameObject cantBuyProductIcon;
    private Product product;

    public void SetProduct(Product newProduct)
    {
        if (newProduct == null || newProduct.count <= 0)
        {
            SetVoid();
            return;
        }

        this.product = newProduct;
        productImage.sprite = newProduct.productImage;
        productName.text = newProduct.productName;
        productCount.text = System.Convert.ToString(newProduct.count);
        productPrice.text = System.Convert.ToString(newProduct.price);

        cantBuyProductIcon.SetActive(!newProduct.GetCanBuy());
    }

    public void SetVoid()
    {
        product = null;
        productImage.sprite = ShopUIM.instance.voidSlotImage;
        productName.text = "пусто";
        productPrice.text = "";
        productCount.text = "";
        cantBuyProductIcon.SetActive(false);
    }

    public void TryBuyProduct()
    {
        if (product.GetCanBuy() && product != null)
        {
            // проверка на наличие деняк !!!

            if (PlayerInventory.instance.money < product.price)
                return;

            product.Buy();
            product.count--;

            PlayerInventory.instance.money -= product.price;
            ShopUIM.instance.UpdatePlayersMoney();

            if (product.count == 0)
            {
                SetVoid();
            }
            else
            {
                productCount.text = System.Convert.ToString(product.count);
            }
        }
        else
        {
            Debug.Log("Cant buy this product!");
        }
    }
}
