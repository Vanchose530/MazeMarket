using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirElephant: PassiveItem
{
    [Header("Settings")]
    public GameObject randomAmmo;
    [Range(0, 100)]
    public int dropChanceInt;
    public override void PassiveItemUsed()
    {
        Debug.Log(1);
        float dropChanceFloat = (float) dropChanceInt / 100;
        Debug.Log(dropChanceFloat);
        Enemy.randomAmmo = randomAmmo;
        Enemy.chanceDrop = dropChanceFloat;
    }

    public override void PickUp()
    {
        PassiveItemUsed();
        PassiveItemManager.passiveItems.Add(PassiveItemsEnum.SouvenirElephant);
        PassiveItemManager.passiveItemsAll.Remove(PassiveItemsEnum.SouvenirElephant);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PassiveItemManager.passiveItemsAll.Contains(PassiveItemsEnum.SouvenirElephant))
            PickUp();
    }
}
