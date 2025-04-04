using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Product", menuName = "Products/Ammo Producte", order = 1)]
public class AmmoProduct : Product
{
    [Header("Ammo")]
    [SerializeField] private int ammoCount;
    [SerializeField] private AmmoTypes ammoType;

    public override void Buy()
    {
        Player.instance.weaponsManager.AddAmmoByType(ammoType, ammoCount);
    }
}
