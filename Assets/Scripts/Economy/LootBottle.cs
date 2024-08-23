using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Bottle", menuName = "Scriptable Objects/LootObjects", order = 3)]
public class LootBottle : LootObject
{
    int count;
    BottleTypes type;
    public override void Loot()
    {
        PlayerInventory.instance.AddBottleByType(type, count);
    }
}
