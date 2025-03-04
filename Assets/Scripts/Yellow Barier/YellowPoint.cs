using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPoint : MonoBehaviour, IDamageable
{
    [Header("HP")]
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [Header("Effects")]
    [SerializeField] private GameData damageEffectPrefab;
    [SerializeField] private SoundEffect damageSE;

    [HideInInspector] public YellowBarier yellowBarier;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            DestroyPoint();
        }
    }

    private void DestroyPoint()
    {
        yellowBarier.DestroyOnePoint();
        Destroy(gameObject);
    }
}
