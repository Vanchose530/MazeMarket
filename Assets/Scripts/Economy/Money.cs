using UnityEngine;

public class Money : Item
{
    [SerializeField] private int _value;
    
    [Header("Attraction")]
    [SerializeField] private int _distanceToAttract;
    [SerializeField] private int _attractForce;
    protected override void PickUp()
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
