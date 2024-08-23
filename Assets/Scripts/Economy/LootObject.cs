
using UnityEngine;

public abstract class LootObject : ScriptableObject
{
    
    public abstract void Loot();
    public virtual bool GetCanLoot() { return true; }


}
