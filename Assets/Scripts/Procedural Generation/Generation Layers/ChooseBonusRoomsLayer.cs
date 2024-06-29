using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Bonus Rooms Layer", menuName = "Generation Layers/Choose Bonus Rooms", order = 5)]
public class ChooseBonusRoomsLayer : GenerationLayer
{
    [Header("Settings")]
    [SerializeField] private double minBonusValueSum;

    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (!room.canHaveBonus)
                continue;

            if (room.roomType == RoomType.Deadlock)
            {
                room.bonusValue = 1;
                Vector2Int transNeighbPos = room.GetTransistedRoomsPositions().ElementAt(0);
                levelTemplate.levelRooms[transNeighbPos.x, transNeighbPos.y].canHaveBonus = false;
            }
            else if (!room.obligatory)
            {
                room.bonusValue = 1;
            }
        }
    }
}
