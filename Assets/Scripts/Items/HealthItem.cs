using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public int health;
    protected override void PickUp()
    {
        Player.instance.health += health;
        AudioManager.instance.PlaySoundEffect(pickUpSE, transform.position, 3f);
        SaveCollectedItem();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Player.instance.health < Player.instance.maxHealth)
            PickUp();
    }
}
