using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBalancerItem : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private Balancer<GameObject> items;
    
    void Start()
    {
        GameObject item = Instantiate(items.Get());
        item.transform.position = this.transform.position;
        item.transform.rotation = Quaternion.identity;
    }
}
