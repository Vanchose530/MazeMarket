using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bottle Product", menuName = "Products/Bottle Product", order = 1)]
public class BottleProduct : Product
{
    [Header("Bottle")]
    [SerializeField] private int bottleCount;
    [SerializeField] private BottleType bottleType;

    public override void Buy()
    {
        switch (bottleType)
        {
            case BottleType.Void:
                PlayerInventory.instance.countEmptyBottle += bottleCount;
                break;
            case BottleType.DemonsBloodGrenade:
                PlayerInventory.instance.countGrenadeBottle += bottleCount;
                break;
            case BottleType.HealthPoition:
                PlayerInventory.instance.countHealthBottle += bottleCount;
                break;
        }
    }
}
