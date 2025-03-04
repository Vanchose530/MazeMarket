using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zlovon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeLife = 2f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float speedDestroy = 0.005f;

    private float scaleX;
    private float scaleY;


    void Start()
    {
        scaleX = radius;

        scaleY = radius;

        gameObject.transform.localScale = new Vector3(scaleX, scaleY);

        gameObject.GetComponent<CircleCollider2D>().radius = radius / 2;

        StartCoroutine("DestroyZlovon");

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
    
    IEnumerator DestroyZlovon()
    {

        while (scaleX > 0)
        {
            scaleX -= speedDestroy;

            scaleY -= speedDestroy;

            transform.localScale = new Vector3(scaleX, scaleY);

            gameObject.GetComponent<CircleCollider2D>().radius -= scaleX / 2;

            yield return new WaitForSeconds(0.01f);
        }

        Destroy(gameObject);

    }
    
}
