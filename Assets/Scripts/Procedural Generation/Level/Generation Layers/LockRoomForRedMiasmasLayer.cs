using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lock Room For Red Miasmas", menuName = "Generation Layers/Lock Room/Red Miasmas", order = 13)]
public class LockRoomForRedMiasmasLayer : GenerationLayer
{
    [Header("Dont Lock Bonus Rooms")]
    [SerializeField] private List<BonusType> dontLockBonusRooms;

    [Header("Bonus Modifier")]
    [SerializeField] private float lockRoomBonusModifier = 1.2f;

    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 1-st try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (!CanLockThisBonusRoom(room))
                continue;

            if (room.bonusValue > 0
                && room.roomType == RoomType.Deadlock
                && !CheckSodaMachineNear(room, levelTemplate)
                && room.lockType == RoomLockType.None
                && room.bonusType != BonusType.DemonsBloodFountain)
            {
                room.lockType = RoomLockType.RedMiasmas;
                room.bonusValue *= lockRoomBonusModifier;
                room.enemyesOnRoom = EnemyesOnRoom.None;
                return;
            }
        }

        foreach (var roomPos in levelTemplate.levelRoomsPositions) // 2-nd try
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (!CanLockThisBonusRoom(room))
                continue;

            if (room.bonusValue > 0
                && !room.obligatory
                && !CheckSodaMachineNear(room, levelTemplate)
                && room.lockType == RoomLockType.None
                && room.bonusType != BonusType.DemonsBloodFountain)
            {
                room.lockType = RoomLockType.RedMiasmas;
                room.bonusValue *= lockRoomBonusModifier;
                room.enemyesOnRoom = EnemyesOnRoom.None;
                return;
            }
        }
    }

    bool CheckSodaMachineNear(RoomTemplate room, LevelTemplate level)
    {
        if (room.bonusType == BonusType.SodaMachine)
            return true;

        HashSet<Vector2Int> trans = room.GetTransistedRoomsPositions();

        foreach (var pos in trans)
        {
            RoomTemplate transRoom = level.levelRooms[pos.x, pos.y];

            if (transRoom.bonusType == BonusType.SodaMachine)
                return true;
        }

        return false;
    }

    bool CanLockThisBonusRoom(RoomTemplate room)
    {
        foreach (var type in dontLockBonusRooms)
        {
            if (room.bonusType == type)
                return false;
        }

        return true;
    }
}
