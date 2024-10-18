using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneThrowState : BronzeHeraclesState
{
    [Header("Behavior")]
    public bool takenStone;
    public bool walkStone;
    public int countStone;
    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.target = Player.instance.transform;

        bronzeHeracles.attack = true;

        bronzeHeracles.isTakeStone = false;

        bronzeHeracles.isShootStone = false;

        bronzeHeracles.isWalkStone = false;

        bronzeHeracles.stand = true;

        bronzeHeracles.targetOnAim = true;

        countStone = bronzeHeracles.RandomStone();

        takenStone = false;
        walkStone = false;
    }
    public override void Run()
    {
        bronzeHeracles.ExecutePath();
        if (countStone > 0)
        {
            if (!takenStone)
            {
                bronzeHeracles.TakeStone();
            }
            else if (takenStone && !walkStone)
            {
                bronzeHeracles.WalkStone();
            }
            else if (takenStone && walkStone)
            {
                bronzeHeracles.ShootStone();
            }
        }
        else 
        { 
            bronzeHeracles.attack = false;
            isFinished = true;
            bronzeHeracles.bodyAnimator.SetTrigger("Default");
            bronzeHeracles.SetState(bronzeHeracles.recoveryState);
        }
    }

}
