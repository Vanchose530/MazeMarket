
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Ammo", menuName = "Scriptable Objects/LootObjects", order = 4)]
public class LootAmmo : LootObject
{
    int count;
    int inaccuracy;
    AmmoTypes type;


    public override void Loot()
    {
        PlayerWeaponsManager.instance.AddAmmoByType(type, Random.Range(count - inaccuracy, count + inaccuracy));
    }

}
