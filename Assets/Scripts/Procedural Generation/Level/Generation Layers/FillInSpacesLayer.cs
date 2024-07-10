using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fill In Spaces Layer", menuName = "Generation Layers/Fill In Spaces", order = 10)]
public class FillInSpacesLayer : GenerationLayer
{
    [Header("Bonus Value")]
    [SerializeField] private float bonusValue = 0.7f;

    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room.canHaveBonus && IsItSpace(room, levelTemplate))
            {
                room.bonusValue = bonusValue;
            }
        }
    }

    bool IsItSpace(RoomTemplate room, LevelTemplate level)
    {
        if (room.bonusType != BonusType.None || room.enemyesOnRoom != EnemyesOnRoom.None)
        {
            return false;
        }

        HashSet<Vector2Int> transRooms = room.GetTransistedRoomsPositions();

        foreach (Vector2Int pos in transRooms)
        {
            RoomTemplate transRoom = level.levelRooms[pos.x, pos.y];

            if (transRoom.bonusType != BonusType.None || transRoom.enemyesOnRoom != EnemyesOnRoom.None)
            {
                return false;
            }
        }

        return true;
    }
}
