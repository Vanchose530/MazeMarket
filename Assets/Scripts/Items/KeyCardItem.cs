using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardItem : Item
{
    public override void PickUp()
    {
        PlayerInventory.instance.keyCardCount++;
        AudioManager.instance.PlaySoundEffect(pickUpSE, transform.position, 3f);
        SaveCollectedItem();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUp();
        }
    }
}
