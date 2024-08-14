using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUIM : MonoBehaviour
{
    public static MiniMapUIM instance { get; private set; }

    [Header("Map Rooms")]
    [SerializeField] private MiniMapRoom deadlockMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom IMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom LMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom TMiniMapRoomPrefab;
    [SerializeField] private MiniMapRoom XMiniMapRoomPrefab;

    List<MiniMapRoom> miniMapRooms;

    [Header("Signs Sprites")]
    [SerializeField] private Sprite startSign;
    [SerializeField] private Sprite endSign;
    [SerializeField] private Sprite mapSign;

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
    [SerializeField] private GameObject mapPanel;
    // [SerializeField] private GameObject mapCanvas;

    public bool isMiniMapActive { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Mini Map UI Manager in scene!");
        instance = this;

        miniMapRooms = new List<MiniMapRoom>();
    }

    public MiniMapRoom BuildMiniMapRoom(Room room)
    {
        MiniMapRoom buildedRoom = null;

        switch (room.roomType)
        {
            case RoomType.Deadlock:
                buildedRoom = Instantiate(deadlockMiniMapRoomPrefab, mapPanel.transform);
                break;
            case RoomType.I_room:
                buildedRoom = Instantiate(IMiniMapRoomPrefab, mapPanel.transform);
                break;
            case RoomType.L_room:
                buildedRoom = Instantiate(LMiniMapRoomPrefab, mapPanel.transform);
                break;
            case RoomType.T_room:
                buildedRoom = Instantiate(TMiniMapRoomPrefab, mapPanel.transform);
                break;
            case RoomType.X_room:
                buildedRoom = Instantiate(XMiniMapRoomPrefab, mapPanel.transform);
                break;
        }

        buildedRoom.lockType = room.lockType;
        buildedRoom.bonusType = room.bonusType;

        buildedRoom.transform.position =
            new Vector3(room.positionInLevel.x + xOffset, room.positionInLevel.y + yOffset)
            * spaceBetweenRooms;

        miniMapRooms.Add(buildedRoom);

        return buildedRoom;
    }

    public void ShowMiniMap()
    {
        mapPanel.SetActive(true);
    }

    public void HideMiniMap()
    {
        mapPanel.SetActive(false);
    }

    public void UseMap()
    {
        // примерный метод
        foreach (var miniMapRoom in miniMapRooms)
        {
            if (miniMapRoom.status == MiniMapRoomStatus.Hidden)
            {
                miniMapRoom.status = MiniMapRoomStatus.VisibleWay;
            }
        }
    }
}
