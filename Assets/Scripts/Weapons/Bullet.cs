using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage;
    float existTime;

    [Header("General")]
    [SerializeField] private bool goingThroughtEnemyes;

    [Header("Effects")]
    [SerializeField] private GameObject bulletsAftereffects;
    [SerializeField] private SoundEffect bulletHitSE;

    [Header("Ignore")]
    [SerializeField] private string ignoreTag;
    [SerializeField] private bool destroyOnIgnore;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    float _existTime;

    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        Collider2D coll = GetComponent<Collider2D>();

        if (goingThroughtEnemyes)
            coll.isTrigger = true;
        else
            coll.isTrigger = false;
    }

    private void Start()
    {
        _existTime = existTime;
    }

    private void Update()
    {
        _existTime -= Time.deltaTime;
        if (_existTime <= 0 )
            Destroy(gameObject);
    }

    public void SetBulletParameters(int damage, float existTime = 10f)
    {
        this.damage = damage;
        this.existTime = existTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ignoreTag))
        {
            if (destroyOnIgnore)
                Destroy(gameObject);

            return;
        }
        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();
        if ( obj != null )
        { 
            obj.TakeDamage(damage, this.transform);
        }
        else
        { 
            EffectsManager.instance.PlayEffect(bulletsAftereffects, gameObject.transform.transform.position, gameObject.transform.rotation, 0.3f);
            // EffectsManager.instance.PlaySoundEffect(bulletHitSoundPrefab, gameObject.transform.position, 3f, 0.7f, 0.9f);
            AudioManager.instance.PlaySoundEffect(bulletHitSE, transform.position, 3f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();

        if (obj != null) { obj.TakeDamage(damage, this.transform); }
        else
        {
            rb.velocity = Vector2.zero;
            EffectsManager.instance.PlayEffect(bulletsAftereffects, gameObject.transform.transform.position, gameObject.transform.rotation, 0.3f);
            AudioManager.instance.PlaySoundEffect(bulletHitSE, transform.position, 3f);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag(ignoreTag))
        {
            if (destroyOnIgnore)
                Destroy(gameObject);

            return;
        }
    }
}
