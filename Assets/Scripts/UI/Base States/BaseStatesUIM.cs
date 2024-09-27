using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatesUIM : MonoBehaviour
{
    public abstract void SetMaxHealth(int health);
    public abstract void SetCurrentHealth(int health);
    public abstract void SetMaxStamina(float stamina);
    public abstract void SetCurrentStamina(float stamina);
    public abstract void SetCanUseStamina(bool canUseStamina);
}

