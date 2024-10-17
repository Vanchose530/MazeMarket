using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Boss In End Room Layer", menuName = "Generation Layers/Set Boss In End Room", order = 10)]
public class SetBossInEndRoomLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        if (levelTemplate.endRoom != null)
        {
            levelTemplate.endRoom.enemyesOnRoom = EnemyesOnRoom.Boss;
        }
        else
        {
            Debug.LogError("There is no end room on level to set boss in!");
        }
    }
}
