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
        if (newProduct == null)
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
        productPrice.text = "0";
        productCount.text = "0";
        cantBuyProductIcon.SetActive(false);
    }

    public void TryBuyProduct()
    {
        if (product.GetCanBuy())
        {
            // проверка на наличие деняк !!!
            product.Buy();
        }
        else
        {
            Debug.Log("Cant buy this product!");
        }
    }
}
