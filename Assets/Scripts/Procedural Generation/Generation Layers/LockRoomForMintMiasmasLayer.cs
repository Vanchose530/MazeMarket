using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lock Room For Mint Miasmas", menuName = "Generation Layers/Lock Room/Mint Miasmas", order = 14)]
public class LockRoomForMintMiasmasLayer : GenerationLayer
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

            if (room.bonusValue > 0 && room.roomType == RoomType.Deadlock && !CheckDemonsBloodFountainNear(room, levelTemplate) && room.lockType == RoomLockType.None)
            {
                room.lockType = RoomLockType.MintMiasmas;
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

            if (room.bonusValue > 0 && !room.obligatory && !CheckDemonsBloodFountainNear(room, levelTemplate) && room.lockType == RoomLockType.None)
            {
                room.lockType = RoomLockType.MintMiasmas;
                room.bonusValue *= lockRoomBonusModifier;
                room.enemyesOnRoom = EnemyesOnRoom.None;
                return;
            }
        }
    }

    bool CheckDemonsBloodFountainNear(RoomTemplate room, LevelTemplate level)
    {
        if (room.bonusType == BonusType.DemonsBloodFountain)
            return true;

        HashSet<Vector2Int> trans = room.GetTransistedRoomsPositions();

        foreach (var pos in trans)
        {
            RoomTemplate transRoom = level.levelRooms[pos.x, pos.y];

            if (transRoom.bonusType == BonusType.DemonsBloodFountain)
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
