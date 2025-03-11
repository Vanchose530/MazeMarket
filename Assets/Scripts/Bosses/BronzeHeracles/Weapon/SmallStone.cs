using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmallStone : Stone
{
    private void Start()
    {
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Bullet")
        {
            IDamageable obj = collision.gameObject.GetComponent<IDamageable>();

            if (obj != null)
            {
                obj.TakeDamage(damageSmallStone, transform);
            }
            AudioManager.instance.PlaySoundEffect(hitSE, transform.position);
            Destroy(gameObject);
        }
    }
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(timeDestroySmallStone);
        Destroy(gameObject);
    }
}
