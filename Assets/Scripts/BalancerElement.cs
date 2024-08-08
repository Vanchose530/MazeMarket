using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BalancerElement<T>
{
    public T element;
    public int chance;

    public BalancerElement(T element, int chance)
    {
        this.element = element;
        this.chance = chance;
    }
}
