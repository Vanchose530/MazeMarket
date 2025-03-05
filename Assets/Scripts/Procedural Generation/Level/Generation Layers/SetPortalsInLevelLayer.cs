using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Portals In Level Layer", menuName = "Generation Layers/Set Portals", order = 10)]
public class SetPortalsInLevelLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPoss in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPoss.x, roomPoss.y];

            if (room.bonusType == BonusType.SodaMachine
                || room.bonusType == BonusType.Shop
                || room.bonusType == BonusType.DemonsBloodFountain)
            {
                if (!HavePortalsNear(room, levelTemplate))
                    room.havePortal = true;
            }
        }

        foreach (var roomPoss in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPoss.x, roomPoss.y];

            if (room.lockType != RoomLockType.None)
            {
                var transRoomsPos = room.GetTransistedRoomsPositions();

                foreach (var transRoom in transRoomsPos)
                {
                    RoomTemplate trRoom = levelTemplate.levelRooms[transRoom.x, transRoom.y];

                    if (!HavePortalsNearWithoutLocks(trRoom, levelTemplate))
                        trRoom.havePortal = true;
                }

                if (room.lockType == RoomLockType.RedMiasmas)
                {
                    room.havePortal = true;
                }
            }
        }

        if (!HavePortalsNear(levelTemplate.endRoom, levelTemplate))
        {
            levelTemplate.endRoom.havePortal = true;
        }

        foreach (var roomPoss in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPoss.x, roomPoss.y];

            if (room.transitionsCount > 1 && !room.havePortal)
            {
                if (!HavePortalsNear(room, levelTemplate))
                    room.havePortal = true;
            }
        }
    }

    private bool HavePortalsNear(RoomTemplate room, LevelTemplate levelTemplate)
    {
        var transRoomsPos = room.GetTransistedRoomsPositions();

        foreach (var transRoom in transRoomsPos)
        {
            RoomTemplate trRoom = levelTemplate.levelRooms[transRoom.x, transRoom.y];

            if (trRoom.havePortal)
            {
                return true;
            }
        }

        return false;
    }

    private bool HavePortalsNearWithoutLocks(RoomTemplate room, LevelTemplate levelTemplate)
    {
        var transRoomsPos = room.GetTransistedRoomsPositions();

        foreach (var transRoom in transRoomsPos)
        {
            RoomTemplate trRoom = levelTemplate.levelRooms[transRoom.x, transRoom.y];

            if (trRoom.havePortal && trRoom.lockType == RoomLockType.None)
            {
                return true;
            }
        }

        return false;
    }
}
