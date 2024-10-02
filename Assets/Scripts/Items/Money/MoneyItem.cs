using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class MoneyItem : Item
{
    [Header("Count")]
    [SerializeField] private int _count;
    public int count { get { return _count; } }

    [Header("Magnetize to Player")]
    [SerializeField] private bool magnetizeToPlayer = true;
    [SerializeField] private float magnetizeToPlayerForce = 1f;

    [Header("Setup")]
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D rb { get { return _rb; } }
    [SerializeField] private CircleCollider2D toPlayerMagnetizeRadius;

    Transform playerTransform;

    private void OnValidate()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (magnetizeToPlayer && playerTransform != null)
        {
            Vector2 force = playerTransform.position - gameObject.transform.position;
            _rb.AddForce(force, ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PickUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerTransform = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerTransform = null;
    }

    protected override void PickUp()
    {
        PlayerInventory.instance.money += count;
        if (pickUpSE != null)
            AudioManager.instance.PlaySoundEffect(pickUpSE, 0.05f);
        Destroy(gameObject);
    }
}
