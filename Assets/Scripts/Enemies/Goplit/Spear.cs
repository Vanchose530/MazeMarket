using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    //const string HIGHT_OBJECTS_TAG = "Hight Decoration";
    //const string WALS_TAG = "Wall";

    [Header("Settings")]
    [SerializeField] private string ignoreTag;
    [SerializeField] private LayerMask stopLayerMask;
    [SerializeField] private int damage;
    [SerializeField] private Goplit goplit;

    private void Awake()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlaySoundEffect(goplit.hitSpearSE, goplit.rb.position);
        if (collision.gameObject.CompareTag(ignoreTag))
            return;

        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
        
        if (obj != null) 
            obj.TakeDamage(damage, transform);

        //if (collision.gameObject.CompareTag(HIGHT_OBJECTS_TAG) || collision.gameObject.CompareTag(WALS_TAG))
        //{
        //    goplit.EndAttack();
        //    // ������ ��������� �����
        //}

        if ((1 << collision.gameObject.layer & stopLayerMask) != 0)
        {
            goplit.EndAttack();
        }
    }
}
