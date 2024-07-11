using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ArcheryState : BronzeHeraclesState
{
    public int countArrow;

    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.target = Player.instance.transform;

        bronzeHeracles.isTakeBow = false;

        bronzeHeracles.isWalkBow = false;

        bronzeHeracles.isShootBow = false;

        bronzeHeracles.targetOnAim = true;

        bronzeHeracles.attack = true;

        bronzeHeracles.stand = true;

        bronzeHeracles.isBowInHand = false;

        countArrow= bronzeHeracles.RandomArrow();
    }
    public override void Run()
    {
        bronzeHeracles.ExecutePath();
        if (!bronzeHeracles.isBowInHand)
            bronzeHeracles.TakeTheBow();
        if (countArrow > 0)
        {
            if (!bronzeHeracles.isBowInHand)
                bronzeHeracles.TakeTheBow();
            else if (bronzeHeracles.isBowInHand && !bronzeHeracles.stand)
            {
                bronzeHeracles.WalkBow();
            }
            else
            {
                bronzeHeracles.ShootBow();
            }
        }
        else {
            bronzeHeracles.RemoveBow();
            isFinished = true;
        }
    }


}
