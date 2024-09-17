using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Money", menuName = "Scriptable Objects/Loot/Money", order = 1)]
public class LootMoney : LootObject
{
    [Header("Loot Money")]
    [SerializeField] private int count;
    [SerializeField] private int inaccuracy;
    int countToAdd = 0;

    public override void Loot()
    {
        if (inaccuracy > 0)
            countToAdd = Random.Range(count - inaccuracy, count + inaccuracy);
        else if (inaccuracy < 0)
            countToAdd = Random.Range(count + inaccuracy, count - inaccuracy);
        else if (inaccuracy == 0)
            countToAdd = count;

        PlayerInventory.instance.money += countToAdd;
    }

    public override string GetLootString()
    {
        return "Δενόγθ " + countToAdd;
    }
}
