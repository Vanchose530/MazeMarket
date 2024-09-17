using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Bottle", menuName = "Scriptable Objects/Loot/Bottle", order = 1)]
public class LootBottle : LootObject
{
    [Header("Loot Bottle")]
    [SerializeField] private int count;
    [SerializeField] private BottleType type;

    public override void Loot()
    {
        switch (type)
        {
            case BottleType.Void:
                PlayerInventory.instance.countEmptyBottle += count;
                break;
            case BottleType.HealthPoition:
                PlayerInventory.instance.countHealthBottle += count;
                break;
            case BottleType.DemonsBloodGrenade:
                PlayerInventory.instance.countGrenadeBottle += count;
                break;
        }
    }

    public override string GetLootString()
    {
        string bottleTypeStr = "";

        switch (type)
        {
            case BottleType.Void:
                bottleTypeStr = "Пустая склянка";
                break;
            case BottleType.HealthPoition:
                bottleTypeStr = "Лечебное зелье";
                break;
            case BottleType.DemonsBloodGrenade:
                bottleTypeStr = "Граната из крови демона лабиринтов";
                break;
        }

        return bottleTypeStr + " " + count;
    }
}
