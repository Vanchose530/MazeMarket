using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeHeraclesDeathState : BronzeHeraclesState
{
    [Header("Behavior")]
    public float deathTime;
    public override void Init()
    {
        isFinished = false;
    }
    public override void Run()
    {
        if (bronzeHeracles.isDeath)
        {
            bronzeHeracles.Death();
        }
        else 
        {
            isFinished = true;
        }
    }
}
