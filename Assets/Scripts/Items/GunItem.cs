using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteGlow;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class GunItem : Item, IInteractable
{
    [Header("Gun Item")]
    public Gun gunSO;
    private Gun gun;

    [Header("Interactable")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;

    private void OnValidate()
    {
        if (pickUpSE.sound == null)
            pickUpSE = gunSO.pickUpSE;
        if (interactSpriteGlow == null)
            interactSpriteGlow = GetComponent<SpriteGlowEffect>();
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
        GetComponent<SpriteRenderer>().sprite = gunSO.image;

        interactSpriteGlow.enabled = false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player")
    //        && !Player.instance.weaponsManager.IsGunSlotsFull())
    //        PickUp();
    //}

    protected override void PickUp() 
    {
        if (Player.instance.weaponsManager.IsGunSlotsFull())
        {
            if (Player.instance.weaponsManager.isDropDelay)
                return;

            if (Player.instance.weaponsManager.weaponInventoryId == 4)
                Player.instance.weaponsManager.ToFirstWeapon();

            Player.instance.weaponsManager.RemoveWeapon();
        }

        AudioManager.instance.PlaySoundEffect(pickUpSE, transform.position, 3f);
        Player.instance.weaponsManager.AddWeapon(gun);
        SaveCollectedItem();
        Destroy(gameObject);
    }

    public void Interact(Player player)
    {
        PickUp();
    }

    public void CanInteract(Player player)
    {
        interactSpriteGlow.enabled = true;
    }

    public void CanNotInteract(Player player)
    {
        interactSpriteGlow.enabled = false;
    }
}
