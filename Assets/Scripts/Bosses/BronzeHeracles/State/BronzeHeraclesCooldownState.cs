using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BronzeHeraclesCooldownState : BronzeHeraclesState
{
    [Header("Behavior")]
    public float cooldownTime;
    public override void Init()
    {
        bronzeHeracles.stand = true;
        bronzeHeracles.targetOnAim = true;
        isFinished = false;
    }
    public override void Run()
    {
        if (bronzeHeracles.isCoolDown && !bronzeHeracles.isCoolDownCoroutine)
        {
            bronzeHeracles.CoolDown();
        }
        else if(!bronzeHeracles.isCoolDown)
        {
            isFinished = true;
            bronzeHeracles.SetState(bronzeHeracles.RandomState());
        }
    }

}
