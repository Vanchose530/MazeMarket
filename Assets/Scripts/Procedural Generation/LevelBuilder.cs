using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] private float spaceBetweenRooms = 1.5f;

    [Header("Level Generator")]
    [SerializeField] private LevelGenerator levelGenerator;

    [Header("Base Rooms")]
    [SerializeField] private List<Room> deadlockRoomPrefabs;
    [SerializeField] private List<Room> iRoomPrefabs;
    [SerializeField] private List<Room> lRoomPrefabs;
    [SerializeField] private List<Room> tRoomPrefabs;
    [SerializeField] private List<Room> xRoomPrefabs;

    [Header("Extra Rooms")]
    [SerializeField] private List<Room> startRoomPrefabs;
    [SerializeField] private List<Room> endRoomPrefabs;

    private void OnValidate()
    {
        if (levelGenerator == null)
            levelGenerator = GetComponent<LevelGenerator>();
    }

    private void Start()
    {
        LevelTemplate levelTemplate = levelGenerator.GenerateNewLevel();

        BuildLevel(levelTemplate);
    }

    void BuildLevel(LevelTemplate level)
    {
        if (level.startRoom != null)
            BuildStartRoom(level.startRoom);

        foreach (var roomPos in level.levelRoomsPositions)
        {
            RoomTemplate room = level.levelRooms[roomPos.x, roomPos.y];

            if (room == level.startRoom || room == level.endRoom)
                continue;

            BuildRoom(room);
        }

        if (level.endRoom != null)
            BuildEndRoom(level.endRoom);

        //foreach (var transition in level.levelTransitions)
        //{
        //    BuildTransition(transition);
        //}
    }
    void SetRoomPosition(Room room, Vector2Int position)
    {
        room.transform.position = new Vector3(position.x + xOffset, position.y + yOffset, 0) * spaceBetweenRooms;
    }

    void BuildRoom(RoomTemplate roomTemplate)
    {
        Room roomPrefab = null;

        switch (roomTemplate.roomType)
        {
            case RoomType.Deadlock:
                roomPrefab = GetRandomRoom(deadlockRoomPrefabs);
                break;
            case RoomType.I_room:
                roomPrefab = GetRandomRoom(iRoomPrefabs);
                break;
            case RoomType.L_room:
                roomPrefab = GetRandomRoom(lRoomPrefabs);
                break;
            case RoomType.T_room:
                roomPrefab = GetRandomRoom(tRoomPrefabs);
                break;
            case RoomType.X_room:
                roomPrefab = GetRandomRoom(xRoomPrefabs);
                break;
        }

        Room newRoom = Instantiate(roomPrefab);

        newRoom.bonusValue = roomTemplate.bonusValue;
        newRoom.bonusType = roomTemplate.bonusType;
        newRoom.enemyesOnRoom = roomTemplate.enemyesOnRoom;
        newRoom.lockType = roomTemplate.lockType;

        SetRoomPosition(newRoom, roomTemplate.position);
        RightRotateRoom(newRoom.transform, roomTemplate);
    }

    void BuildStartRoom(RoomTemplate roomTemplate)
    {
        Room roomPrefab = GetRandomRoom(startRoomPrefabs);

        Room newRoom = Instantiate(roomPrefab);

        newRoom.bonusValue = roomTemplate.bonusValue;
        newRoom.bonusType = roomTemplate.bonusType;
        newRoom.enemyesOnRoom = roomTemplate.enemyesOnRoom;
        newRoom.lockType = roomTemplate.lockType;

        SetRoomPosition(newRoom, roomTemplate.position);
        RightRotateRoom(newRoom.transform, roomTemplate);
    }

    void BuildEndRoom(RoomTemplate roomTemplate)
    {
        Room roomPrefab = GetRandomRoom(endRoomPrefabs);

        Room newRoom = Instantiate(roomPrefab);

        newRoom.bonusValue = roomTemplate.bonusValue;
        newRoom.bonusType = roomTemplate.bonusType;
        newRoom.enemyesOnRoom = roomTemplate.enemyesOnRoom;
        newRoom.lockType = roomTemplate.lockType;

        SetRoomPosition(newRoom, roomTemplate.position);
        RightRotateRoom(newRoom.transform, roomTemplate);
    }

    private void RightRotateRoom(Transform roomTransform, RoomTemplate roomTemplate)
    {
        int zRotation = 0;

        switch (roomTemplate.roomType) // поворот некоторых комнат можно рандомить
        {
            case RoomType.Deadlock:
                if (roomTemplate.transitionUp != null)
                    zRotation = 180;
                else if (roomTemplate.transitionRight != null)
                    zRotation = 90;
                else if (roomTemplate.transitionDown != null)
                    zRotation = 0;
                else if (roomTemplate.transitionLeft != null)
                    zRotation = 270;
                break;
            case RoomType.I_room:
                if (roomTemplate.transitionUp != null/* && roomTemplate.transitionDown != null*/)
                    zRotation = 0;
                if (roomTemplate.transitionLeft != null/* && roomTemplate.transitionRight != null*/)
                    zRotation = 90;
                break;
            case RoomType.L_room:
                if (roomTemplate.transitionUp != null && roomTemplate.transitionRight != null)
                    zRotation = 0;
                else if (roomTemplate.transitionRight != null && roomTemplate.transitionDown != null)
                    zRotation = 270;
                else if (roomTemplate.transitionDown != null && roomTemplate.transitionLeft != null)
                    zRotation = 180;
                else if (roomTemplate.transitionLeft != null && roomTemplate.transitionUp != null)
                    zRotation = 90;
                break;
            case RoomType.T_room:
                if (roomTemplate.transitionUp == null)
                    zRotation = 0;
                else if (roomTemplate.transitionRight == null)
                    zRotation = 270;
                else if (roomTemplate.transitionDown == null)
                    zRotation = 180;
                else if (roomTemplate.transitionLeft == null)
                    zRotation = 90;
                break;
            case RoomType.X_room:
                zRotation = 0;
                break;
        }

        // Debug.Log("Room type: " + roomTemplate.roomType + "      Z Rotator: " + zRotation);

        //roomTransform.rotation = new Quaternion(roomTransform.gameObject.transform.rotation.x,
        //    roomTransform.gameObject.transform.rotation.y, zRotation, 0);

        roomTransform.Rotate(roomTransform.rotation.x, roomTransform.rotation.y, zRotation);
    }

    private Room GetRandomRoom(List<Room> rooms)
    {
        if (rooms.Count == 0)
            return null;
        else if (rooms.Count == 1)
            return rooms[0];

        int r = Random.Range(0, rooms.Count);

        return rooms[r];
    }

}
