using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ArcheryState : BronzeHeraclesState
{
    [Header("Behavior")]
    public int countArrow;
    public float timeTakeBow;
    public float timeNewArrow;
    public float timeWalkBow;
    public float timeAimingBow;
    public float timeToShootBow;
    public float timeRemoveBow;
    public int minCountArrow;
    public int maxCountArrow;
    public float forceArrow;
    public int damageArrow;

    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.target = Player.instance.transform;

        bronzeHeracles.targetOnAim = true;

        bronzeHeracles.attack = true;

        bronzeHeracles.stand = true;

        bronzeHeracles.isBowInHand = false;

        countArrow = bronzeHeracles.RandomArrow();
    }
    public override void Run()
    {
        bronzeHeracles.ExecutePath();
        if (countArrow > 0)
        {
            if (!bronzeHeracles.isBowInHand && !bronzeHeracles.isTakeBow)
                bronzeHeracles.TakeTheBow();
            else if (bronzeHeracles.isBowInHand && !bronzeHeracles.stand && !bronzeHeracles.isWalkBow)
            {
                bronzeHeracles.WalkBow();
            }
            else if(bronzeHeracles.stand && bronzeHeracles.isBowInHand && !bronzeHeracles.isShootBow)
            {
                bronzeHeracles.ShootBow();
            }
        }
        else if(countArrow == 0 && !bronzeHeracles.isRemoveBow) 
        {
            bronzeHeracles.RemoveBow();
            isFinished = true;
        }
    }
}
