using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Shop Layer", menuName = "Generation Layers/Choose Bonus Type/Shop", order = 9)]
public class ChooseShopLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 1-st try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room.roomType == RoomType.Deadlock && room.bonusValue >= 1 && room.bonusType == BonusType.Chest)
            {
                room.bonusType = BonusType.Shop;
                return;
            }
        }

        Debug.Log("Cant find Deadlock with 1 bonus value count. Try to find another Deadlock with bonus");

        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 2-nd try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room.roomType == RoomType.Deadlock && room.bonusValue > 0 && room.bonusType == BonusType.Chest)
            {
                room.bonusType = BonusType.Shop;
                return;
            }
        }

        Debug.Log("Cant find Deadlock with bonus. Try to find another room with bonus and make it deadlock");

        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 3-rd try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room.bonusValue > 0 && room.bonusType == BonusType.Chest)
            {
                HashSet<Vector2Int> transRoomsPos = room.GetTransistedRoomsPositions();
                int transObligatoryRoomsCount = 0;

                foreach (var transRoomPos in transRoomsPos)
                {
                    RoomTemplate transRoom = levelTemplate.levelRooms[transRoomPos.x, transRoomPos.y];
                    if (transRoom.obligatory)
                        transObligatoryRoomsCount++;
                }

                if (transObligatoryRoomsCount == room.transitionsCount)
                {
                    continue;
                }
                else
                {
                    foreach (var transRoomPos in transRoomsPos)
                    {
                        RoomTemplate transRoom = levelTemplate.levelRooms[transRoomPos.x, transRoomPos.y];
                        if (!transRoom.obligatory)
                            levelTemplate.DestroyTransition(room, transRoom);
                    }

                    if (room.transitionsCount > 1)
                    {
                        foreach (var transRoomPos in transRoomsPos)
                        {
                            RoomTemplate transRoom = levelTemplate.levelRooms[transRoomPos.x, transRoomPos.y];
                            levelTemplate.DestroyTransition(room, transRoom);

                            if (room.transitionsCount == 1)
                                break;
                        }
                    }
                }

                room.bonusType = BonusType.Shop;
                return;
            }
        }
    }
}
