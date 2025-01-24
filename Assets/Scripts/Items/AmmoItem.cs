using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : Item
{
    [Header("Ammo Item")]
    public AmmoTypes ammoType;
    public int ammoCount;

    [Header("Setup")]
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D rb { get { return _rb; } }

    private void OnValidate()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }

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
