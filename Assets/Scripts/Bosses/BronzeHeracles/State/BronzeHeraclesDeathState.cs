using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeHeraclesDeathState : BronzeHeraclesState
{
    [Header("Behavior")]
    public float deathTime;
    public override void Init()
    {
        bronzeHeracles.targetOnAim = false;
        bronzeHeracles.stand = true;
        isFinished = false;
    }
    public override void Run()
    {
        if (bronzeHeracles.isDeath && !bronzeHeracles.isDeathCoroutine)
        {
            bronzeHeracles.Death();
        }
        else if(!bronzeHeracles.isDeath)
        {
            isFinished = true;
        }
    }
}
