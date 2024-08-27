using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private int damageArrow;
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
            Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}

