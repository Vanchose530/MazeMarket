using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    public static List<PassiveItemsEnum> passiveItems = new List<PassiveItemsEnum>();
    public static List<PassiveItemsEnum> passiveItemsAll = new List<PassiveItemsEnum>() { PassiveItemsEnum.Fizzy, PassiveItemsEnum.PirateFlag, PassiveItemsEnum.SouvenirElephant };
}
