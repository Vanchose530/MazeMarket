using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Balancer<T>
{
    public List<BalancerElement<T>> elements;

    public void Add(BalancerElement<T> element)
    {
        elements.Add(element);
    }

    public void Add(T element, int chance)
    {
        elements.Add(new BalancerElement<T>(element, chance));
    }

    public T Get()
    {
        int sum = 0;

        foreach (var element in elements)
        {
            sum += element.chance;
        }

        int r = UnityEngine.Random.Range(0, sum);
        
        int localSum = 0;

        foreach (var element in elements)
        {
            if (r < element.chance + localSum)
            {
                return element.element;
            }
            localSum += element.chance;
        }

        return elements[0].element;
    }
}
