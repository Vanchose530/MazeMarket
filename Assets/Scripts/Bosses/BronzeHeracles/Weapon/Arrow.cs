using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ArcheryState
{
    [Header("Sound")]
    [SerializeField] private SoundEffect hitSE;
    private void Start()
    {
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();

        if (obj != null)
        {
            obj.TakeDamage(damageArrow, transform);
        }
        AudioManager.instance.PlaySoundEffect(hitSE, transform.position);
        Destroy(gameObject);
    }
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}

