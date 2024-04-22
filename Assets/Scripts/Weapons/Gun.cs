using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Gun", menuName = "Scriptable Objects/Weapons/Gun", order = 1)]
public class Gun : Weapon
{
    [Header("Gun Settings")]
    public int magazineSize;
    public float reloadTime;
    public float firingRate;
    public float spread;
    public float bulletForce;
    public AmmoTypes ammoType;
    [SerializeField] private GameObject bulletPrefab;
    [HideInInspector] public int ammoInMagazine;
    [HideInInspector] public bool reloading;

    [Header("Gun Sound Effects")]
    [SerializeField] private SoundEffect shootSE;
    [SerializeField] private SoundEffect emptySE;
    [SerializeField] private SoundEffect _reloadingSE;
    public SoundEffect reloadingSE { get { return _reloadingSE; } }

    public override void Attack(Transform attackPoint)
    {
        if(ammoInMagazine > 0 && !reloading)
        {
            Vector3 bulletAngle = attackPoint.eulerAngles;
            bulletAngle.z += UnityEngine.Random.Range(-spread, spread);

            GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.Euler(bulletAngle));
            Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();

            bullet.GetComponent<Bullet>().SetBulletParameters(damage);
            brb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse); 

            ammoInMagazine--;

            AudioManager.instance.PlaySoundEffect(shootSE, (1 / firingRate) * 1.5f);
        }
        else if (!reloading)
        {
            AudioManager.instance.PlaySoundEffect(emptySE, 1 / firingRate);
        }

        if (onAttack != null)
            onAttack();
    }
}
