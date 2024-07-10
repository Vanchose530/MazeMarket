
using UnityEngine;

public class InvObjBottle : InventoryObject
{
    public int emptyBottleCount;
    public override void ToPlayerInventory()
    {
        PlayerInventory.instance.countEmptyBottle += 1;
    }
}
