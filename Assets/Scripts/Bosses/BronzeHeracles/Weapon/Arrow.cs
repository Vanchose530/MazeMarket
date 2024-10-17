using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private int damageArrow;
    [Header("Sound")]
    [SerializeField] private SoundEffect hitSE;
    private void Start()
    {
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();

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

