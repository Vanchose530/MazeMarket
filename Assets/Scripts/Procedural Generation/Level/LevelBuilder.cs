using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    public static LevelBuilder instance { get; private set; }

    public LevelTemplate levelTemplate { get; private set; }

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

    [Header("Setup")]
    [SerializeField] private AstarPath pathfinder;

    private void OnValidate()
    {
        if (levelGenerator == null)
            levelGenerator = GetComponent<LevelGenerator>();
    }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Level Builder in scene");
        instance = this;
    }

    private void Start()
    {
        levelTemplate = levelGenerator.GenerateNewLevel();

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

        pathfinder.Scan();
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

        SetRoomInfo(newRoom, roomTemplate);

        SetRoomPosition(newRoom, roomTemplate.position);
        RightRotateRoom(newRoom, roomTemplate);
    }

    void SetRoomInfo(Room room, RoomTemplate roomTemplate)
    {
        room.bonusValue = roomTemplate.bonusValue;
        room.bonusType = roomTemplate.bonusType;
        room.enemyesOnRoom = roomTemplate.enemyesOnRoom;
        room.lockType = roomTemplate.lockType;
    }

    void BuildStartRoom(RoomTemplate roomTemplate)
    {
        Room roomPrefab = GetRandomRoom(startRoomPrefabs);

        Room newRoom = Instantiate(roomPrefab);

        SetRoomPosition(newRoom, roomTemplate.position);
        RightRotateRoom(newRoom, roomTemplate);

        StartCoroutine(SetStartRoomCamera(newRoom));
    }

    IEnumerator SetStartRoomCamera(Room startRoom)
    {
        yield return new WaitForSeconds(0.1f);
        startRoom.virtualCameraTrigger.SetVirtualCamera();
    }

    void BuildEndRoom(RoomTemplate roomTemplate)
    {
        Room roomPrefab = GetRandomRoom(endRoomPrefabs);

        Room newRoom = Instantiate(roomPrefab);

        SetRoomPosition(newRoom, roomTemplate.position);
        RightRotateRoom(newRoom, roomTemplate);
    }

    private void RightRotateRoom(Room room, RoomTemplate roomTemplate)
    {
        int zRotation = 0;

        switch (roomTemplate.roomType)
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

        int randRotate = 0;

        if (roomTemplate.roomType == RoomType.I_room)
        {
            int[] rotateAngles = { 0, 180 };
            int r = UnityEngine.Random.Range(0, rotateAngles.Length);
            randRotate = rotateAngles[r];
        }
        else if (roomTemplate.roomType == RoomType.X_room)
        {
            int[] rotateAngles = { 0, 90, 180, 270 };
            int r = UnityEngine.Random.Range(0, rotateAngles.Length);
            randRotate = rotateAngles[r];
        }

        room.transform.Rotate(room.transform.rotation.x, room.transform.rotation.y, zRotation + randRotate);

        try
        {
            room.virtualCameraTrigger.virtualCamera.transform
                .Rotate(room.transform.rotation.x, room.transform.rotation.y, (zRotation + randRotate) * -1);
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Room dont have Virtual Camera Trigger to rotate room virtual camera!");
        }

        try
        {
            room.enemyWavesManager.virtualCamera.transform
                .Rotate(room.transform.rotation.x, room.transform.rotation.y, (zRotation + randRotate) * -1);
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Room dont have Enemy Waves Manager to rotate enemy waves virtual camera!");
        }
        
    }

    private Room GetRandomRoom(List<Room> rooms)
    {
        if (rooms.Count == 0)
            return null;
        else if (rooms.Count == 1)
            return rooms[0];

        int r = UnityEngine.Random.Range(0, rooms.Count);

        return rooms[r];
    }

}
