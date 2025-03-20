using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fizzy : PassiveItem
{
    [Header("Settings")]
    public float increaseRadius;
    public override void PassiveItemUsed()
    {
        float exp = Grenade.explotionRadiusStatic;
        Grenade.explotionRadiusStatic = exp * increaseRadius;
    }

    public override void PickUp()
    {
        PassiveItemUsed();
        PassiveItemManager.passiveItems.Add(PassiveItemsEnum.Fizzy);
        PassiveItemManager.passiveItemsAll.Remove(PassiveItemsEnum.Fizzy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PassiveItemManager.passiveItemsAll.Contains(PassiveItemsEnum.Fizzy))
            PickUp();
    }
}
