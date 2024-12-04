using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Ammo", menuName = "Scriptable Objects/Loot/Ammo", order = 1)]
public class LootAmmo : LootObject
{
    [Header("Loot Ammo")]
    [SerializeField] private int count;
    [SerializeField] private int inaccuracy;
    [SerializeField] private AmmoTypes type;
    int countToAdd = 0;

    public override void Loot()
    {
        if (inaccuracy > 0)
            countToAdd = Random.Range(count - inaccuracy, count + inaccuracy);
        else if (inaccuracy < 0)
            countToAdd = Random.Range(count + inaccuracy, count - inaccuracy);
        else if (inaccuracy == 0)
            countToAdd = count;

        Player.instance.weaponsManager.AddAmmoByType(type, countToAdd);
    }

    public override string GetLootString()
    {
        string ammoTypeStr = "";

        switch (type)
        {
            case AmmoTypes.LightBullets:
                ammoTypeStr = "Патроны лёгкого калибра";
                break;
            case AmmoTypes.MediumBullets:
                ammoTypeStr = "Патроны среднего калибра";
                break;
            case AmmoTypes.HeavyBullets:
                ammoTypeStr = "Патроны тяжёлого калибра";
                break;
            case AmmoTypes.Shells:
                ammoTypeStr = "Дроби";
                break;
        }

        return ammoTypeStr + " " + countToAdd;
    }
}
