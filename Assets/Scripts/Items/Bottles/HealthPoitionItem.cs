using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoitionItem : Item
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }

    protected override void PickUp()
    {
        PlayerInventory.instance.countHealthBottle++;
        SaveCollectedItem();

        if (pickUpSE != null)
            AudioManager.instance.PlaySoundEffect(pickUpSE, 3f);

        Destroy(gameObject);
    }
}
