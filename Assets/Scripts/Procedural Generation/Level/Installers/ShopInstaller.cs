using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInstaller : MonoBehaviour
{
    [Header("Install Settings")]
    [SerializeField] private int productsCount1 = 3;
    [SerializeField] private float border1 = 0.8f;
    [SerializeField] private int productsCount2 = 4;
    [SerializeField] private float border2 = 1.2f;
    [SerializeField] private int productsCount3 = 5;

    [Header("Setup")]
    [SerializeField] private Shop shop;

    public void Install(float value)
    {
        if (value < border1)
        {
            shop.productsCount = productsCount1;
        }
        else if (value >= border1 && value < border2)
        {
            shop.productsCount = productsCount2;
        }
        else if (value >= border2)
        {
            shop.productsCount = productsCount3;
        }
    }
}
