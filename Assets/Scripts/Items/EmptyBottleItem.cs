using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBottleItem : Item
{



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player")) {
            PickUp();
        }

    }

    public override void PickUp()
    {
        PlayerInventory.instance.countEmptyBottle++;
        SaveCollectedItem();
        Destroy(gameObject);
    }
}
