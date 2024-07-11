using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private int damage;
    private void Start()
    {
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}

