using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Make Transitions Layer", menuName = "Generation Layers/Make Transitions", order = 3)]
public class MakeTransitionsLayer : GenerationLayer
{
    public override void Layer(LevelTemplate levelTemplate)
    {
        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (!room.canMakeTransitions)
                continue;

            HashSet<Vector2Int> neighboursPos = room.GetNeighboursPositions();

            foreach (var neighborPos in neighboursPos)
            {
                RoomTemplate neighbourRoom = levelTemplate.levelRooms[neighborPos.x, neighborPos.y];

                if (neighbourRoom.canMakeTransitions && !room.GetHaveTransitionWithRoom(neighbourRoom))
                {
                    levelTemplate.MakeTransition(roomPos, neighborPos);
                }

                if (!room.canMakeTransitions)
                    break;
            }
        }

        // штука для того, чтобы на выходе из начальной комнаты не было бонусов
        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room == levelTemplate.startRoom)
                continue;

            if (room.GetHaveTransitionWithRoom(levelTemplate.startRoom))
            {
                room.canHaveBonus = false;
                break;
            }
        }
    }
}
