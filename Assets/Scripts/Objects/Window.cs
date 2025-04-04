using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Window : MonoBehaviour, IDamageable
{
    [Header("General")]
    public int startEndurance;
    private int endurance;

    [Header("Damage Effect")]
    public float indicateDamageSeconds;
    public Color hurtColor;
    private Color defaultColor;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject brokeEffectPrefab;

    [Header("Sound Effects")]
    [SerializeField] private SoundEffect damageSoundSE;
    [SerializeField] private SoundEffect brokeSoundSE;

    private void OnValidate()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        defaultColor = sr.color;
        endurance = startEndurance;
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        endurance -= damage;
        if (endurance <= 0) Broke(attack);
        else
        {
            // EffectsManager.instance.PlaySoundEffect(damageSoundPrefab, transform.position, 3f, 0.9f, 1.1f);
            AudioManager.instance.PlaySoundEffect(damageSoundSE, transform.position, 3f);
            StartCoroutine(IndicateDamage());
        }
    }

    private void Broke(Transform attack)
    {
        if (attack != null)
        {
            Vector3 effectScale = new Vector3(1, 1, 1);
            
            bool condToReverse = GetReverseEffectCondition(attack);

            if (!condToReverse)
                effectScale = new Vector3(effectScale.x, -1, 1);

            var effect = Instantiate(brokeEffectPrefab, transform.position, transform.rotation);

            effect.transform.localScale = effectScale;

            Destroy(effect, 20f);

            // EffectsManager.instance.PlaySoundEffect(brokeSoundPrefab, transform.position, 20f, 0.8f, 1.5f);
            AudioManager.instance.PlaySoundEffect(brokeSoundSE, transform.position, 15f);
        }
        
        Destroy(gameObject);
    }

    bool GetReverseEffectCondition(Transform attack)
    {
        float difference = transform.rotation.eulerAngles.z - attack.rotation.eulerAngles.z;

        return (difference > -90 && difference <= 90) || (difference > -270 && difference < -550);
    }

    private IEnumerator IndicateDamage()
    {
        sr.color = hurtColor;
        yield return new WaitForSeconds(indicateDamageSeconds);
        sr.color = defaultColor;
    }
}
