using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : Item
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmmoItem"/> class. 
    /// [GeneratedType accepts equals int of <see cref="AmmoTypes"/>]
    /// </summary>
    /// <param name="GeneratedType">GeneratedType accepts equals int of <see cref="AmmoTypes"/></param>
    /// <param name="GeneretedAmmoCount"></param>
    public AmmoItem(int GeneratedType, int GeneretedAmmoCount)
    {
        ammoType = (AmmoTypes) GeneratedType;
        ammoCount = GeneretedAmmoCount;
    }

    public AmmoTypes ammoType;
    public int ammoCount;

    public override void PickUp()
    {
        PlayerWeaponsManager.instance.AddAmmoByType(ammoType, ammoCount);
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
