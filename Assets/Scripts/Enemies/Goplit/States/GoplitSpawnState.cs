using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoplitSpawnState : GoplitState
{
    public override void Init()
    {
        goplit.Spawn();
    }
}
