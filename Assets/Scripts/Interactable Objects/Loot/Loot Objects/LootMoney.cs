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
        int countToAdd = 0;

        if (inaccuracy > 0)
            countToAdd = Random.Range(count - inaccuracy, count + inaccuracy);
        else if (inaccuracy < 0)
            countToAdd = Random.Range(count + inaccuracy, count - inaccuracy);
        else if (inaccuracy == 0)
            countToAdd = count;

        PlayerInventory.instance.money += countToAdd;
    }
}
