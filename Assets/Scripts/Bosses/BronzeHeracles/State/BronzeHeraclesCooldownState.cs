using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeHeraclesCooldownState : BronzeHeraclesState
{
    [Header("Behavior")]
    public float cooldownTime;
    public override void Init()
    {
        isFinished = false;
    }
    public override void Run()
    {
        if (bronzeHeracles.isCoolDown)
        {
            bronzeHeracles.CoolDownScream();
        }
        else 
        {
            isFinished = true;
            bronzeHeracles.SetState(bronzeHeracles.RandomState());
        }
    }

}
