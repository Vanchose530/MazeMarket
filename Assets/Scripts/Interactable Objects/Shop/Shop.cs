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
            Debug.Log("Trying to create products in shop but it was already created");
            return;
        }

        products = new Product[productsSlotsInShop];

        if (productsSlotsInShop <= productsCount)
        {
            if (productsSlotsInShop < productsCount)
                Debug.Log("Products count in shop more than products slots in UI");

            for (int i = 0; i < productsSlotsInShop; i++)
            {
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
                    products[r] = Instantiate(productBalancer.Get());
                    i++;
                }
            }
        }
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
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
        {
            return;
        }

        if (products == null)
            CreateProducts(ShopUIM.instance.slotsCount);

        ShopManager.instance.StartShoping(products);
    }
}
