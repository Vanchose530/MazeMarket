using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallOfGoplits : BronzeHeraclesState
{
    [Header("Behavior")]
    public float callGoplitsTime;
    public override void Init()
    {
        isFinished = false;
        bronzeHeracles.stand = true;
        bronzeHeracles.targetOnAim = false;
    }
    public override void Run()
    {
        bronzeHeracles.CallOfGoplits();
    }
    public override void Exit()
    {
        bronzeHeracles.stand = false;
        isFinished = true;
    }
}
