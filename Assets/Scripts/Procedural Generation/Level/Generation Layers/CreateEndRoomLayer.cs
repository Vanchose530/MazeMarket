using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Create End Room Layer", menuName = "Generation Layers/Create End Room", order = 4)]
public class CreateEndRoomLayer : GenerationLayer
{
    [Header("Setings")]
    [SerializeField] private bool createTransitionToEndRoom = false;
    public override void Layer(LevelTemplate levelTemplate)
    {
        RoomTemplate farFromStartRoom = GetFarRoomFromStart(levelTemplate);

        if (farFromStartRoom.neighboursCount >= 4)
            Debug.LogWarning("Far from start room neighbours count equal 4! Cant create end room near!");

        Vector2Int farFromStartRoomPos = farFromStartRoom.position;
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

        if (farFromStartRoomPos.x > 0 && levelTemplate.levelRooms[farFromStartRoomPos.x - 1, farFromStartRoomPos.y] == null)
            vacantPlaces.Add(new Vector2Int(farFromStartRoomPos.x - 1, farFromStartRoomPos.y));
        if (farFromStartRoomPos.y > 0 && levelTemplate.levelRooms[farFromStartRoomPos.x, farFromStartRoomPos.y - 1] == null)
            vacantPlaces.Add(new Vector2Int(farFromStartRoomPos.x, farFromStartRoomPos.y - 1));
        if (farFromStartRoomPos.x < levelTemplate.maxX && levelTemplate.levelRooms[farFromStartRoomPos.x + 1, farFromStartRoomPos.y] == null)
            vacantPlaces.Add(new Vector2Int(farFromStartRoomPos.x + 1, farFromStartRoomPos.y));
        if (farFromStartRoomPos.y < levelTemplate.maxY && levelTemplate.levelRooms[farFromStartRoomPos.x, farFromStartRoomPos.y + 1] == null)
            vacantPlaces.Add(new Vector2Int(farFromStartRoomPos.x, farFromStartRoomPos.y + 1));

        if (vacantPlaces.Count > 0)
        {
            Vector2Int endRoomPos = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            levelTemplate.endRoom = levelTemplate.CreateRoomAtPosition(endRoomPos.x, endRoomPos.y);

            if (createTransitionToEndRoom)
            {
                levelTemplate.MakeTransition(endRoomPos, farFromStartRoomPos);
            }

            // farFromStartRoom.canHaveBonus = false;
        }
        else
        {
            Debug.Log("Cant find vacant places to far from start room to create end room. Chose far from start room to be end room");

            HashSet<Vector2Int> transRoomsPos = farFromStartRoom.GetTransistedRoomsPositions();

            bool findDeadlock = false;

            foreach (var transRoomPos in transRoomsPos)
            {
                RoomTemplate transRoom = levelTemplate.levelRooms[transRoomPos.x, transRoomPos.y];

                if (transRoom.roomType == RoomType.Deadlock)
                {
                    levelTemplate.endRoom = transRoom;
                    findDeadlock = true;
                    break;
                }
            }

            if (!findDeadlock && farFromStartRoom.transitionsCount > 1)
            {
                levelTemplate.endRoom = farFromStartRoom;

                foreach (var transRoomPos in transRoomsPos)
                {
                    RoomTemplate transRoom = levelTemplate.levelRooms[transRoomPos.x, transRoomPos.y];

                    if (transRoom.roomType != RoomType.Deadlock)
                    {
                        levelTemplate.DestroyTransition(farFromStartRoom.position, transRoom.position);
                    }

                    if (farFromStartRoom.transitionsCount == 1)
                        break;
                }
            }
        }
        
        levelTemplate.endRoom.canHaveBonus = false;
        levelTemplate.endRoom.maxTransitionsCount = 1;
    }

    RoomTemplate GetFarRoomFromStart(LevelTemplate levelTemplate)
    {
        Vector2Int farFromStartRoomPos = Vector2Int.zero;
        float maxDistanceToStartingRoom = 0;

        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            float aSq = Mathf.Pow(roomPos.x - levelTemplate.startRoom.position.x, 2);
            float bSq = Mathf.Pow(roomPos.y - levelTemplate.startRoom.position.y, 2);

            float distanceToStartingRoom = Mathf.Sqrt(aSq + bSq);

            if (distanceToStartingRoom > maxDistanceToStartingRoom )
            {
                farFromStartRoomPos = roomPos;
                maxDistanceToStartingRoom = distanceToStartingRoom;
            }
        }

        RoomTemplate farFromStartRoom = levelTemplate.levelRooms[farFromStartRoomPos.x, farFromStartRoomPos.y];
        return farFromStartRoom;
    }
}
