using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBalancerItem : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private int chanceOfAppearanceAnyItem = 50;

    [Header("Items")]
    [SerializeField] private Balancer<GameObject> items;
    
    void Start()
    {
        int r = Random.Range(0, 100);

        if (r < chanceOfAppearanceAnyItem)
        {
            GameObject item = Instantiate(items.Get());
            item.transform.position = this.transform.position;
            item.transform.rotation = Quaternion.identity;
        }
    }
}
