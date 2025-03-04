using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamageable
{
    public int startEndurance;
    private int endurance;
    public float indicateDamageSeconds;
    public Color hurtColor;
    private Color defaultColor;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        endurance = startEndurance;
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        StartCoroutine(IndicateDamage());
        endurance -= damage;
        if (endurance <= 0 ) Destroy(gameObject);
    }

    private IEnumerator IndicateDamage()
    {
        sr.color = hurtColor;
        yield return new WaitForSeconds(indicateDamageSeconds);
        sr.color = defaultColor;
    }
}
