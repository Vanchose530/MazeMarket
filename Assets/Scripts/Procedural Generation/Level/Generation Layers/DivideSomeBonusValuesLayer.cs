using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Divide Some Bonus Values Layer", menuName = "Generation Layers/Divide Some Bonus Values", order = 6)]
public class DivideSomeBonusValuesLayer : GenerationLayer
{
    HashSet<RoomTemplate> dividedRooms = new HashSet<RoomTemplate>();

    public override void Layer(LevelTemplate levelTemplate)
    {
        dividedRooms.Clear();

        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (dividedRooms.Contains(room))
                continue;

            HashSet<Vector2Int> transNeighboursPos = room.GetTransistedRoomsPositions();

            HashSet<RoomTemplate> transistedBonusRooms = new HashSet<RoomTemplate>();
            transistedBonusRooms.Add(room);

            if (room.bonusValue > 0)
            {
                foreach (var transNeighbPos in transNeighboursPos)
                {
                    RoomTemplate transistedRoom = levelTemplate.levelRooms[transNeighbPos.x, transNeighbPos.y];
                    if (transistedRoom.bonusValue == room.bonusValue)
                        transistedBonusRooms.Add(transistedRoom);
                    else if (transistedRoom.bonusValue > 0 && transistedRoom.bonusValue != room.bonusValue && !transistedRoom.obligatory && !room.obligatory)
                    {
                        levelTemplate.DestroyTransition(room, transistedRoom);
                    }
                }

                if (transistedBonusRooms.Count == 1)
                {
                    continue;
                }
                else if (transistedBonusRooms.Count > 1)
                {
                    DivideBonusValueForRooms(room.bonusValue, transistedBonusRooms.ToArray());
                }
            }
        }
    }

    void DivideBonusValueForRooms(float dividedValue, params RoomTemplate[] rooms)
    {
        float value = dividedValue / rooms.Length;
        foreach (var room in rooms)
        {
            dividedRooms.Add(room);
            room.bonusValue = value;
        }
    }
}
