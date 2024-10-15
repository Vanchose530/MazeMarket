using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    [Header("Shop")]
    public int productsCount = 3;
    [SerializeField] private Balancer<Product> productBalancer;
    Product[] products;
    [SerializeField] private bool uniqueProducts;
    const int UNIQUE_TRYES = 20;

    [Header("Interact Effect")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [SerializeField] private SoundEffect interactSE;

    void Start()
    {
        interactSpriteGlow.enabled = false;
    }

    void CreateProducts(int productsSlotsInShop)
    {
        if (products != null)
        {
            Debug.LogError("Trying to create products in shop but it was already created");
            return;
        }

        products = new Product[productsSlotsInShop];

        if (productsSlotsInShop <= productsCount)
        {
            if (productsSlotsInShop < productsCount)
                Debug.Log("Products count in shop more than products slots in UI");

            for (int i = 0; i < productsSlotsInShop; i++)
            {
                if (uniqueProducts)
                {
                    bool findUniqueProduct = false;

                    for (int j = 0; j < UNIQUE_TRYES; j++)
                    {
                        var prod = productBalancer.Get();
                        if (CheckForUnique(prod))
                        {
                            products[i] = Instantiate(prod);
                            findUniqueProduct = true;
                            break;
                        }
                    }

                    if (findUniqueProduct)
                        break;

                    Debug.LogWarning("Cant find unique product in Shop after " + UNIQUE_TRYES.ToString() + " tryes!");
                }
                products[i] = Instantiate(productBalancer.Get());
            }
        }
        else
        {
            for (int i = 0; i < productsCount;)
            {
                int r = Random.Range(0, productsSlotsInShop);

                if (products[r] == null)
                {
                    if (uniqueProducts)
                    {
                        bool findUniqueProduct = false;

                        for (int j = 0; j < UNIQUE_TRYES; j++)
                        {
                            var prod = productBalancer.Get();
                            Debug.Log(prod.productName);
                            Debug.Log(CheckForUnique(prod));
                            if (CheckForUnique(prod))
                            {
                                products[r] = Instantiate(prod);
                                findUniqueProduct = true;
                                break;
                            }
                        }

                        if (findUniqueProduct)
                        {
                            i++;
                            continue;
                        }
                            
                        Debug.LogWarning("Cant find unique product in Shop after " + UNIQUE_TRYES.ToString() + " tryes!");
                    }
                    products[r] = Instantiate(productBalancer.Get());
                    i++;
                }
            }
        }
    }

    private bool CheckForUnique(Product product)
    {
        if (products.Length == 0)
            return true;
        foreach (Product p in products)
        {
            if (p == null)
                continue;
            if (product.productName == p.productName) // ÑÐÀÂÍÅÍÈÅ ÏÎ ÈÌÅÍÀÌ!
                return false;
        }
        return true;
    }

    public void CanInteract(Player player)
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
        {
            return;
        }

        interactSpriteGlow.enabled = true;
    }

    public void CanNotInteract(Player player)
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
        {
            return;
        }

        interactSpriteGlow.enabled = false;
    }

    public void Interact(Player player)
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle
            || PlayerConditionsManager.instance.currentCondition == PlayerConditions.Shoping)
        {
            return;
        }

        if (products == null)
            CreateProducts(ShopUIM.instance.slotsCount);

        ShopManager.instance.StartShoping(products);
    }
}
