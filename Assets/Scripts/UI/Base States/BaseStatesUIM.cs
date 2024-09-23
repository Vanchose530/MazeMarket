using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatesUIM : MonoBehaviour
{
    public abstract void SetMaxHealth(int health);
    public abstract void SetCurrentHealth(int health);
    public abstract void SetMaxStamina(int stamina);
    public abstract void SetCurrentStamina(int stamina);
}

