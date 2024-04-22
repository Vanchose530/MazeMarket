using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnState : ZombieState
{
    public override void Init()
    {
        zombie.Spawn();
    }
}
