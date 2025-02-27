using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpittingBloodSpawnState : ZombieSpittingBloodState
{
    public override void Init()
    {
        zombieSpittingBlood.Spawn();
    }
}
