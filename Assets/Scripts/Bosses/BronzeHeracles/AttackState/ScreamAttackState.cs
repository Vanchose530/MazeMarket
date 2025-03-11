using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamAttackState : BronzeHeraclesState
{
    public float timePreparing;
    public float timeScream;
    public float timeDamping;
    public Vector3 targetSize;
    public GameObject scream;

    public override void Init()
    {
        isFinished = false;
        bronzeHeracles.stand = true;
        bronzeHeracles.targetOnAim = false;
        scream.SetActive(false);
    }
    public override void Run()
    {
        if (!bronzeHeracles.isScreamAttack) 
        {
            bronzeHeracles.ScreamAttack();
        }
    }
    public override void Exit()
    {
        bronzeHeracles.stand = false;
        isFinished = true;
    }
}
