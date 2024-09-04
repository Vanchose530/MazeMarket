using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LootObject : ScriptableObject
{
    public abstract void Loot();

    public virtual bool GetCanLoot() { return true; }
    public virtual string GetLootString() { return "~ отсутствует строка лута ~"; }
}
