using UnityEngine;

public class Money : Item
{
    [SerializeField] private int _value;
    
    public Money(int value)
    {
        _value = value;
    }

    [Header("Attraction")] //dont work now
    [SerializeField] private int _distanceToAttract;
    [SerializeField] private int _attractForce;
    public override void PickUp()
    {
        PlayerInventory.instance.moneyCount += _value;
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
