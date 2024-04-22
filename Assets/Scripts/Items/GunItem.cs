using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class GunItem : Item
{
    public Gun gunSO;
    private Gun gun;

    private void OnValidate()
    {
        if (pickUpSE.sound == null)
            pickUpSE = gunSO.pickUpSE;
    }

    private void Start()
    {
        Invoke("CheckToCollected", 0.01f); 

        if (gunSO == null)
        {
            Debug.LogWarning("Not setted gun for Gun Item");
            Destroy(gameObject);
        }

        gun = Instantiate(gunSO);
        gun.ammoInMagazine = gun.magazineSize;
        GetComponent<SpriteRenderer>().sprite = gun.image;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PickUp();
    }

    protected override void PickUp() 
    {
        if (PlayerWeaponsManager.instance.IsGunSlotsFull())
        {
            Debug.Log("Inventory is full of guns");
            return;
        }
        else
            AudioManager.instance.PlaySoundEffect(pickUpSE, transform.position, 3f);
            PlayerWeaponsManager.instance.AddWeapon(gun);
            SaveCollectedItem();
            Destroy(gameObject); 
    }
}
