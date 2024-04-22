using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueSpawnState : StatueState
{
    public override void Init()
    {
        statue.Spawn();
    }
}
