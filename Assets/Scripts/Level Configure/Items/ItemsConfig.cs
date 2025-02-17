using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Config", menuName = "Scriptable Objects/Configs/Items", order = 1)]
public class ItemsConfig : ScriptableObject
{
    [Header("Appear Chances")]
    [Range(0, 100)]
    [SerializeField] private int _itemSourceAppearChance = 50;
    public int itemSourceAppearChance { get { return _itemSourceAppearChance; } }
    [Range(0, 100)]
    [SerializeField] private int _itemAppearChance = 50;
    public int itemAppearChance { get { return _itemAppearChance; } }

    [Header("Items")]
    [SerializeField] private Balancer<GameObject> items;

    public GameObject GetItemGameObject()
    {
        return items.Get();
    }
}
