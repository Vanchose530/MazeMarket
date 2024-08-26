using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Product : ScriptableObject
{
    [Header("Product")]
    [SerializeField] private string _productName;
    public string productName { get { return _productName; } }
    [SerializeField] private Sprite _productImage;
    public Sprite productImage { get { return _productImage; } }
    [SerializeField] private int _price;
    public int price { get { return _price; } }
    [SerializeField] private int _count;
    public int count { get { return _count; } }
    public int currentCount { get; set; }

    abstract public void Buy();

    virtual public bool GetCanBuy() { return true; }
}
