using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Money", menuName = "Scriptable Objects/Loot/Money", order = 1)]
public class LootMoney : LootObject
{
    [Header("Loot Money")]
    [SerializeField] private int count;
    [SerializeField] private int inaccuracy;

    public override void Loot()
    {
        // а мани то пока что нет
    }
}
