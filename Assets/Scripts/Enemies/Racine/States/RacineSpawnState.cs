using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacineSpawnState : RacineState
{
    public override void Init()
    {
        racine.Spawn();
    }
}
