using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAmmo : LootObject
{
    [Header("Loot Ammo")]
    [SerializeField] private int count;
    [SerializeField] private int inaccuracy;
    [SerializeField] private AmmoTypes type;

    public override void Loot()
    {
        int countToAdd = 0;

        if (inaccuracy > 0)
            countToAdd = Random.Range(count - inaccuracy, count + inaccuracy);
        else if (inaccuracy < 0)
            countToAdd = Random.Range(count + inaccuracy, count - inaccuracy);
        else if (inaccuracy == 0)
            countToAdd = count;

        PlayerWeaponsManager.instance.AddAmmoByType(type, countToAdd);
    }
}
