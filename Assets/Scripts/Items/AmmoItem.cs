using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : Item
{
    public AmmoTypes ammoType;
    public int ammoCount;

    protected override void PickUp()
    {
        Player.instance.weaponsManager.AddAmmoByType(ammoType, ammoCount);
        AudioManager.instance.PlaySoundEffect(pickUpSE, transform.position, 3f);
        SaveCollectedItem();
        Destroy(gameObject); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
            PickUp();
    }
}
