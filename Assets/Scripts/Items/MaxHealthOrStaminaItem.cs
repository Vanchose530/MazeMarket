using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthOrStaminaItem : Item
{
    enum HealthOrStamina
    {
        Health,
        Stamina
    }

    [Header("Chocolate Bar")]
    [SerializeField] private HealthOrStamina healthOrStamina;
    [SerializeField] private int addValue;

    protected override void PickUp()
    {
        switch (healthOrStamina)
        {
            case HealthOrStamina.Health:
                Player.instance.maxHealth += addValue;
                break;
            case HealthOrStamina.Stamina:
                Player.instance.maxStamina += addValue;
                break;
        }

        if (pickUpSE != null)
            AudioManager.instance.PlaySoundEffect(pickUpSE, transform.position, 3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PickUp();
    }
}
