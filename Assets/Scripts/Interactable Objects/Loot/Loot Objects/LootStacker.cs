using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Stacker", menuName = "Scriptable Objects/Loot/Stacker", order = 1)]
public class LootStacker : LootObject
{
    [Header("Loot To Stuck")]
    [SerializeField] private List<LootObject> lootToStuck;

    public override void Loot()
    {
        foreach (var loot in lootToStuck)
        {
            loot.Loot();
        }
    }

    public override string GetLootString()
    {
        string resString = string.Empty;

        foreach (var loot in lootToStuck)
        {
            resString += loot.GetLootString();
            resString += "\n";
        }

        return resString;
    }
}
