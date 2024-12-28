using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonsBloodGrenadeItem : Item
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
        PlayerInventory.instance.countGrenadeBottle++;
        SaveCollectedItem();

        if (pickUpSE != null)
            AudioManager.instance.PlaySoundEffect(pickUpSE, 3f);

        Destroy(gameObject);
    }
}
