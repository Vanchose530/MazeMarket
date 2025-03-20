using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public static PassiveItem instance { get; private set; }
    public virtual void PassiveItemUsed() { }
    public virtual void PickUp() { }
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Passive Item in scene");
        instance = this;
    }
}
