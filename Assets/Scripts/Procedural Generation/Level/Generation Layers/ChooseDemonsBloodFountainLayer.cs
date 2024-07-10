using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Demons Blood Fountain Layer", menuName = "Generation Layers/Choose Bonus Type/Demons Blood Fountain", order = 11)]
public class ChooseDemonsBloodFountainLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        Dictionary<Vector2Int, float> bonusRoomsPositions = levelTemplate.GetBonusRoomsPositions();

        var sortedBonusRoomsPositions = from entry in bonusRoomsPositions orderby entry.Value ascending select entry;

        foreach (var roomPos in sortedBonusRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.Key.x, roomPos.Key.y];

            HashSet<Vector2Int> transRoomsPos = room.GetTransistedRoomsPositions();

            bool haveTransSodaMachine = false;

            foreach (var transPos in transRoomsPos)
            {
                RoomTemplate transRoom = levelTemplate.levelRooms[transPos.x, transPos.y];

                if (transRoom.bonusType == BonusType.SodaMachine)
                {
                    haveTransSodaMachine = true;
                    break;
                }
            }

            if (haveTransSodaMachine)
                continue;

            room.bonusType = BonusType.DemonsBloodFountain;
            return;
        }
    }
}
