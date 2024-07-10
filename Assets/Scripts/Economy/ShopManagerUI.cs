using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    public static ShopManagerUI instance { get; private set; }

    [Header("General")]
    [SerializeField] private GameObject _shopMenuPanel;
    [Header("Product 1")]
    [SerializeField] private GameObject _product1;
    [SerializeField] private TextMeshProUGUI _product1Name;
    [SerializeField] private TextMeshProUGUI _product1Description;
    [SerializeField] private TextMeshProUGUI _product1Count;
    [SerializeField] private TextMeshProUGUI _product1PriceOnButton;
    [SerializeField] private Image _product1Image;
    public string p1NameTxt
    {
        get { return _product1Name.text; }
        set { _product1Name.text = value; }
    }
    public string p1DescTxt
    {
        get { return _product1Description.text; }
        set { _product1Description.text = value; }
    }
    public string p1Count
    {
        get { return _product1Count.text; }
        set { _product1Count.text = value; }
    }
    public string p1Price
    {
        get { return _product1PriceOnButton.text; }
        set { _product1PriceOnButton.text = value; }
    }
    [Header("Product 2")]
    [SerializeField] private GameObject _product2;
    [SerializeField] private TextMeshProUGUI _product2Name;
    [SerializeField] private TextMeshProUGUI _product2Description;
    [SerializeField] private TextMeshProUGUI _product2Count;
    [SerializeField] private TextMeshProUGUI _product2PriceOnButton;
    [SerializeField] private Image _product2Image;

    public string p2NameTxt
    {
        get { return _product2Name.text; }
        set { _product2Name.text = value; }
    }
    public string p2DescTxt
    {
        get { return _product2Description.text; }
        set { _product2Description.text = value; }
    }
    public string p2Count
    {
        get { return _product2Count.text; }
        set { _product2Count.text = value; }
    }
    public string p2Price
    {
        get { return _product2PriceOnButton.text; }
        set { _product2PriceOnButton.text = value; }
    }

    [Header("Product 3")]
    [SerializeField] private GameObject _product3;
    [SerializeField] private TextMeshProUGUI _product3Name;
    [SerializeField] private TextMeshProUGUI _product3Description;
    [SerializeField] private TextMeshProUGUI _product3Count;
    [SerializeField] private TextMeshProUGUI _product3PriceOnButton;
    [SerializeField] private Image _product3Image;
    public string p3NameTxt
    {
        get { return _product3Name.text; }
        set { _product3Name.text = value; }
    }
    public string p3DescTxt
    {
        get { return _product3Description.text; }
        set { _product3Description.text = value; }
    }
    public string p3Count
    {
        get { return _product3Count.text; }
        set { _product3Count.text = value; }
    }
    public string p3Price
    {
        get { return _product3PriceOnButton.text; }
        set { _product3PriceOnButton.text = value; }
    }

    [Header("Product 4")]
    [SerializeField] private GameObject _product4;
    [SerializeField] private TextMeshProUGUI _product4Name;
    [SerializeField] private TextMeshProUGUI _product4Description;
    [SerializeField] private TextMeshProUGUI _product4Count;
    [SerializeField] private TextMeshProUGUI _product4PriceOnButton;
    [SerializeField] private Image _product4Image;
    public string p4NameTxt
    {
        get { return _product4Name.text; }
        set { _product4Name.text = value; }
    }
    public string p4DescTxt
    {
        get { return _product4Description.text; }
        set { _product4Description.text = value; }
    }
    public string p4Count
    {
        get { return _product4Count.text; }
        set { _product4Count.text = value; }
    }
    public string p4Price
    {
        get { return _product4PriceOnButton.text; }
        set { _product4PriceOnButton.text = value; }
    }

    [Header("Product 5")]
    [SerializeField] private GameObject _product5;
    [SerializeField] private TextMeshProUGUI _product5Name;
    [SerializeField] private TextMeshProUGUI _product5Description;
    [SerializeField] private TextMeshProUGUI _product5Count;
    [SerializeField] private TextMeshProUGUI _product5PriceOnButton;
    [SerializeField] private Image _product5Image;
    public string p5NameTxt
    {
        get { return _product5Name.text; }
        set { _product5Name.text = value; }
    }
    public string p5DescTxt
    {
        get { return _product5Description.text; }
        set { _product5Description.text = value; }
    }
    public string p5Count
    {
        get { return _product5Count.text; }
        set { _product5Count.text = value; }
    }
    public string p5Price
    {
        get { return _product5PriceOnButton.text; }
        set { _product5PriceOnButton.text = value; }
    }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one ShopManagerUI script in scene");
        instance = this;
    }

    public void GetProductsFromShop(List<InventoryObject> products, int[] counterOfProducts)
    {
        p1NameTxt = products[0].displayName;
        p1DescTxt = products[0].description;
        p1Price = products[0].purchasePrice.ToString();
        _product1Image.sprite = products[0].image;
        p1Count = counterOfProducts[0].ToString();
        //p2
        p2NameTxt = products[1].displayName;
        p2DescTxt = products[1].description;
        p2Price = products[1].purchasePrice.ToString();
        _product2Image.sprite = products[1].image;
        p2Count = counterOfProducts[1].ToString();
        //p3
        p3NameTxt = products[2].displayName;
        p3DescTxt = products[2].description;
        p3Price = products[2].purchasePrice.ToString();
        _product3Image.sprite = products[2].image;
        p3Count = counterOfProducts[2].ToString();
        //p4
        p4NameTxt = products[3].displayName;
        p4DescTxt = products[3].description;
        p4Price = products[3].purchasePrice.ToString();
        _product4Image.sprite = products[3].image;
        p4Count = counterOfProducts[3].ToString();
        //p5
        p5NameTxt = products[4].displayName;
        p5DescTxt = products[4].description;
        p5Price = products[4].purchasePrice.ToString();
        _product5Image.sprite = products[4].image;
        p5Count = counterOfProducts[4].ToString();
    }

}
