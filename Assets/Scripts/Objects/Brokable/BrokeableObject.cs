using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeableObject : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int minHealth = 100;
    int currentHealth;

    [Header("Effects")]
    [SerializeField] private BrokeEffect brokeEffect;
    [SerializeField] private DamageParticles damageParticlesPrefab;
    [SerializeField] private SoundEffect damageSE;
    [SerializeField] private SoundEffect fullBrokeSE;
    [SerializeField] private GameObject fullBrokeEffectPrefab;

    private void OnValidate()
    {
        brokeEffect = brokeEffect == null ? GetComponentInChildren<BrokeEffect>() : brokeEffect;
    }

    private void Start()
    {
        currentHealth = Random.Range(minHealth, maxHealth);

        brokeEffect.SetEffectStateByRelation(GetHealthRelation());
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (fullBrokeSE != null)
                AudioManager.instance.PlaySoundEffect(fullBrokeSE);

            if (fullBrokeEffectPrefab != null)
            {
                var fbe = Instantiate(fullBrokeEffectPrefab);
                fbe.transform.position = transform.position;
                fbe.transform.rotation = transform.rotation;
                Destroy(fbe, 20f);
            }

            Destroy(gameObject);
            return;
        }

        if (attack != null)
        {
            Vector3 effectScale = new Vector3(1, 1, 1);

            bool condToReverse = GetReverseEffectCondition(attack);

            if (!condToReverse)
                effectScale = new Vector3(effectScale.x, -1, 1);

            var dp = Instantiate(damageParticlesPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), transform.rotation * Quaternion.Euler(0, 0, 90));

            dp.transform.localScale = effectScale;

            Destroy(dp, 1f);
            dp.Play(damage);
        }

        if (damageSE != null)
            AudioManager.instance.PlaySoundEffect(damageSE);

        brokeEffect.SetEffectStateByRelation(GetHealthRelation());
    }

    bool GetReverseEffectCondition(Transform attack)
    {
        float difference = transform.rotation.eulerAngles.z - attack.rotation.eulerAngles.z;

        return (difference > -90 && difference <= 90) || (difference > -270 && difference < -550);
    }

    float GetHealthRelation() => (float)currentHealth / (float)maxHealth;
}
