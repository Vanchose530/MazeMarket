
using UnityEngine;
[CreateAssetMenu(fileName = "New Loot Money", menuName = "Scriptable Objects/LootObjects", order = 2)]
public class LootMoney : LootObject
{
    int count;
    int inaccuracy;

    public override void Loot()
    {
        PlayerInventory.instance.moneyCount += Random.Range(count - inaccuracy, count + inaccuracy);
    }
}
