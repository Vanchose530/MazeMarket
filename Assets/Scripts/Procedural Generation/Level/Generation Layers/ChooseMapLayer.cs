using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Map Layer", menuName = "Generation Layers/Choose Bonus Type/Map", order = 10)]
public class ChooseMapLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 1-st try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (!room.obligatory && room.bonusValue < 1 && room.bonusType == BonusType.Chest)
            {
                room.bonusType = BonusType.Map;
                return;
            }
        }

        Debug.Log("Cant find non obligatory room with bonus value less than 1. Try to find another non obligatory room");

        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 2-nd try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (!room.obligatory && room.bonusType == BonusType.Chest)
            {
                room.bonusType = BonusType.Map;
                return;
            }
        }

        Debug.Log("Cant find non obligatory room. Try to find non Deadlock room");

        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 3-rd try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room.roomType != RoomType.Deadlock && room.bonusType == BonusType.Chest)
            {
                room.bonusType = BonusType.Map;
                return;
            }
        }
    }
}
