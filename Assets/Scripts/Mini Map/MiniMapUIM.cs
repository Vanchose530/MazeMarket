using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MiniMapUIM : MonoBehaviour
{
    public static MiniMapUIM instance { get; private set; }

    [Header("Map Rooms")]
    [SerializeField] private MiniMapRoom deadlockMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom IMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom LMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom TMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom XMiniMapRoomPrefab;

    List<MiniMapRoom> miniMapRoomsList;

    [Header("Signs Sprites")]
    [SerializeField] private Sprite _startSign;
    public Sprite startSign {  get { return _startSign; } }
    [SerializeField] private Sprite _endSign;
    public Sprite endSign { get { return _endSign; } }
    [SerializeField] private Sprite _mapSign;
    public Sprite mapSign { get { return _mapSign; } }
    [SerializeField] private Sprite _chestSign;
    public Sprite chestSign { get { return _chestSign; } }
    [SerializeField] private Sprite _shopSign;
    public Sprite shopSign { get { return _shopSign; } }
    [SerializeField] private Sprite _demonsBloodFountainSign;
    public Sprite demonsBloodFountainSign { get { return _demonsBloodFountainSign; } }
    [SerializeField] private Sprite _sodaMachineSign;
    public Sprite sodaMachineSign { get { return _sodaMachineSign; } }

    [Header("Signs Size")]
    [SerializeField] private float signSize = 1;

    [Header("Player in Room")]
    [SerializeField] private GameObject playerInRoomMark;

    [Header("Settings")]
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] private float roomSizeX = 1;
    [SerializeField] private float roomSizeY = 1;
    [SerializeField] private float spaceBetweenRooms = 1;

    [Header("Setup")]
    [SerializeField] private GameObject mapRooms;
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject mapMark;

    public bool isMiniMapActive { get; private set; }

    MiniMapRoom[,] miniMapRooms;

    public bool mapEnable { get { return mapPanel.active; } }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Mini Map UI Manager in scene!");
        instance = this;

        miniMapRoomsList = new List<MiniMapRoom>();
        playerInRoomMark.transform.localScale = new Vector3(roomSizeX, roomSizeY, -10);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        HideMiniMap();
    }

    public MiniMapRoom BuildMiniMapRoom(Room room)
    {
        MiniMapRoom prefab = null;

        switch (room.roomType)
        {
            case RoomType.Deadlock:
                // buildedRoom = Instantiate(deadlockMiniMapRoomPrefab, mapPanel.transform);
                prefab = deadlockMiniMapRoomPrefab;
                break;
            case RoomType.I_room:
                // buildedRoom = Instantiate(IMiniMapRoomPrefab, mapPanel.transform);
                prefab = IMiniMapRoomPrefab;
                break;
            case RoomType.L_room:
                // buildedRoom = Instantiate(LMiniMapRoomPrefab, mapPanel.transform);
                prefab = LMiniMapRoomPrefab;
                break;
            case RoomType.T_room:
                // buildedRoom = Instantiate(TMiniMapRoomPrefab, mapPanel.transform);
                prefab = TMiniMapRoomPrefab;
                break;
            case RoomType.X_room:
                // buildedRoom = Instantiate(XMiniMapRoomPrefab, mapPanel.transform);
                prefab = XMiniMapRoomPrefab;
                break;
        }

        MiniMapRoom buildedRoom = Instantiate(prefab, mapRooms.transform) as MiniMapRoom;

        //try { buildedRoom.lockType = room.lockType; } catch (NullReferenceException) { }
        //try { buildedRoom.bonusType = room.bonusType; } catch (NullReferenceException) { }

        buildedRoom.lockType = room.lockType;
        buildedRoom.bonusType = room.bonusType;

        buildedRoom.positionInLevel = room.positionInLevel;

        buildedRoom.transform.localPosition =
            new Vector3(room.positionInLevel.x + xOffset, room.positionInLevel.y + yOffset)
            * spaceBetweenRooms;

        buildedRoom.gameObject.transform.localScale = new Vector3(roomSizeX, roomSizeY, 1);

        RightRotateMiniMapRoom(buildedRoom, room);

        buildedRoom.status = MiniMapRoomStatus.Hidden;
        buildedRoom.playerStatus = MiniMapRoomPlayerStatus.WasNotIn;

        miniMapRoomsList.Add(buildedRoom);

        if (miniMapRooms == null)
        {
            int x = LevelBuilder.instance.levelTemplate.levelRooms.GetLength(0);
            int y = LevelBuilder.instance.levelTemplate.levelRooms.GetLength(1);
            miniMapRooms = new MiniMapRoom[x, y];
        }

        miniMapRooms[buildedRoom.positionInLevel.x, buildedRoom.positionInLevel.y] = buildedRoom;

        return buildedRoom;
    }

    void RightRotateMiniMapRoom(MiniMapRoom mimiMapRoom, Room room)
    {
        int zRotation = 0;

        RoomTemplate roomTemplate = LevelBuilder.instance.levelTemplate.levelRooms[room.positionInLevel.x, room.positionInLevel.y];

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

        mimiMapRoom.transform.Rotate(mimiMapRoom.transform.rotation.x, mimiMapRoom.transform.rotation.y, zRotation);
    }

    public void SetSignOnMiniMapRoom(MiniMapRoom room, Sprite sign)
    {
        if (sign == null)
        {
            Debug.LogWarning("Cant set sign on room because there are no sign sprite");
            return;
        }

        Image roomSign = Instantiate(new GameObject(), room.transform).AddComponent<Image>();
        roomSign.sprite = sign;
        roomSign.transform.localPosition = new Vector3(0, 0, 5);
        roomSign.transform.localScale = new Vector3(signSize, signSize, 1);
        roomSign.transform.rotation = Quaternion.identity;
    }

    public void SetPlayerInRoomMark(MiniMapRoom miniMapRoom)
    {
        playerInRoomMark.transform.position = miniMapRoom.transform.position;
    }

    public void ShowMiniMap()
    {
        mapPanel.SetActive(true);
        mapMark.SetActive(false);
    }

    public void HideMiniMap()
    {
        mapPanel.SetActive(false);
        mapMark.SetActive(true);
    }

    public void UseMap()
    {
        foreach (var miniMapRoom in miniMapRoomsList)
        {
            if (miniMapRoom.status == MiniMapRoomStatus.Hidden)
            {
                miniMapRoom.status = MiniMapRoomStatus.VisibleWay;

                if (miniMapRoom.locksCount == 1) // если комната является тупиковой
                {
                    RoomTemplate roomTemplate
                        = LevelBuilder.instance.levelTemplate.
                        levelRooms[miniMapRoom.positionInLevel.x, miniMapRoom.positionInLevel.y];

                    if (roomTemplate == LevelBuilder.instance.levelTemplate.endRoom)
                    {
                        miniMapRoom.status = MiniMapRoomStatus.VisibleWayAndBonus;
                    }
                }
            }
        }
    }

    public void ShowRoomsNear(Room room)
    {
        RoomTemplate roomTemplate =
            LevelBuilder.instance.levelTemplate.levelRooms
            [room.positionInLevel.x, room.positionInLevel.y];

        HashSet<Vector2Int> transRoomPositions = roomTemplate.GetTransistedRoomsPositions();

        foreach (var pos in transRoomPositions)
        {
            MiniMapRoom roomNear = miniMapRooms[pos.x, pos.y];

            if (roomNear.status == MiniMapRoomStatus.Hidden)
            {
                roomNear.status = MiniMapRoomStatus.VisibleWay;
            }
        }
    }
}
