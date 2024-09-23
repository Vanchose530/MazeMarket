using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryUIM : MonoBehaviour
{
    public abstract void SetAmmoByType(int count, AmmoTypes type);
    public abstract void SetVoidBottle(int count);
    public abstract void SetMoney(int count);
    public abstract void OpenInventory();
    public abstract void CloseInventory();
}
