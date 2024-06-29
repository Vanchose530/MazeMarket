using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cheking Minimum Bonus Rooms Layer", menuName = "Generation Layers/Cheking Minimum Bonus Rooms", order = 7)]
public class ChekingMinimumBonusRoomsLayer : GenerationLayer
{
    [Header("Minimum Bonus Rooms")]
    public int minimumBonusRoomsCount;

    public override void Layer(LevelTemplate levelTemplate)
    {
        int bonusRoomsCount = 0;

        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];
            if (room.bonusValue > 0)
                bonusRoomsCount++;
        }

        Debug.Log("Cant reach the minimum of the bonus value sum by smart way. Using random way");

        if (bonusRoomsCount < minimumBonusRoomsCount)
        {
            List<Vector2Int> roomPositions = new List<Vector2Int>();

            foreach (var roomPos in levelTemplate.levelRoomsPositions)
            {
                roomPositions.Add(roomPos);
            }
            
            while (roomPositions.Count > 0 && bonusRoomsCount < minimumBonusRoomsCount)
            {
                Vector2Int randRoomPos = roomPositions[Random.Range(0, roomPositions.Count)];
                RoomTemplate randRoom = levelTemplate.levelRooms[randRoomPos.x, randRoomPos.y];

                if (randRoom.bonusValue == 0 && randRoom.canHaveBonus)
                {
                    randRoom.bonusValue = 1;
                    bonusRoomsCount++;
                }
                    
                roomPositions.Remove(randRoomPos);
            }
        }
    }
}
