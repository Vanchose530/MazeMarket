using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Soda Machine Layer", menuName = "Generation Layers/Choose Bonus Type/Soda Machine", order = 12)]
public class ChooseSodaMachineLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        Dictionary<Vector2Int, float> bonusRoomsPositions = levelTemplate.GetBonusRoomsPositions();

        var sortedBonusRoomsPositions = from entry in bonusRoomsPositions orderby entry.Value ascending select entry;

        foreach (var roomPos in sortedBonusRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.Key.x, roomPos.Key.y];

            HashSet<Vector2Int> transRoomsPos = room.GetTransistedRoomsPositions();

            bool haveTransDemonsBloodFountain = false;

            foreach (var transPos in transRoomsPos)
            {
                RoomTemplate transRoom = levelTemplate.levelRooms[transPos.x, transPos.y];

                if (transRoom.bonusType == BonusType.DemonsBloodFountain)
                {
                    haveTransDemonsBloodFountain = true;
                    break;
                }
            }

            if (haveTransDemonsBloodFountain)
                continue;

            room.bonusType = BonusType.SodaMachine;
            return;
        }
    }
}
