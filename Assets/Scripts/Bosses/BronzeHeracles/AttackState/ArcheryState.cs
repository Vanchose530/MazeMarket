using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ArcheryState : BronzeHeraclesState
{
    

    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.target = Player.instance.transform;

        bronzeHeracles.targetOnAim = true;

        bronzeHeracles.attack = true;

        bronzeHeracles.TakeTheBow();
    }
    public override void Run()
    {
        bronzeHeracles.ExecutePath();

        if (!bronzeHeracles.isShootBow)
            bronzeHeracles.ShootBow();

    }


}
