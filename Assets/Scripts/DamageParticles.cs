using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticles : MonoBehaviour
{
    [Header("Life time")]
    [SerializeField] private float lifeTimeOnDamage = 0; // если параметр равен 0, то он не дейтсвителен
    [SerializeField] private float lifeTimeOffset = 0;
    [SerializeField] private float minLifeTime = 0;
    [SerializeField] private float maxLifeTime = 10000;

    [Header("Speed")]
    [SerializeField] private float speedOnDamage = 0.75f; // если параметр равен 0, то он не дейтсвителен
    [SerializeField] private float speedOffset = 0;
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 10000;

    [Header("Setup")]
    [SerializeField] private ParticleSystem particles;

    private void OnValidate()
    {
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Play(int damage, Transform attack = null)
    {
        if (lifeTimeOnDamage > 0) 
        {
            float value = (damage / lifeTimeOnDamage) + lifeTimeOffset;
            particles.startLifetime = Mathf.Clamp(value, minLifeTime, maxLifeTime);
        }
            
        if (speedOnDamage > 0)
        {
            float value = (damage / speedOnDamage) + speedOffset;
            particles.startSpeed = Mathf.Clamp(value, minSpeed, maxSpeed);
        }

        if (attack != null)
            particles.transform.rotation = attack.rotation;

        particles.Play();
    }
}
