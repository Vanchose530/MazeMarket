using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Passive Item", menuName = "Scriptable Objects/Loot/PassiveItem", order = 1)]
public class LootPassiveItem : LootObject
{
    [SerializeField] private PassiveItemsEnum passiveItem;
    public override void Loot()
    {
            switch (passiveItem)
            {
                case PassiveItemsEnum.Fizzy:
                    PassiveItemManager.passiveItems.Add(passiveItem);
                    Fizzy.instance.PassiveItemUsed();
                    break;
                case PassiveItemsEnum.SouvenirElephant:
                    PassiveItemManager.passiveItems.Add(passiveItem);
                    SouvenirElephant.instance.PassiveItemUsed();
                    break;
            }
        PassiveItemManager.passiveItemsAll.Remove(passiveItem);
    }
    public override string GetLootString()
    {
        string passiveItemTypeStr = "";

        switch (passiveItem)
        {
            case PassiveItemsEnum.Fizzy:
                passiveItemTypeStr = "Ўипучка";
                break;
            case PassiveItemsEnum.SouvenirElephant:
                passiveItemTypeStr = "—увенирный слоник";
                break;
        }

        return passiveItemTypeStr;
    }
}
