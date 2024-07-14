using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIM : MonoBehaviour
{
    public static MapUIM instance { get; private set; }

    [Header("Map Tiles")]
    [SerializeField] private Sprite deadlockRoomMapTile;
    [SerializeField] private Sprite IRoomMapTile;
    [SerializeField] private Sprite LRoomMapTile;
    [SerializeField] private Sprite TRoomMapTile;
    [SerializeField] private Sprite XRoomMapTile;

    [Header("Signs Sprites")]
    [SerializeField] private Sprite startSign;
    [SerializeField] private Sprite endSign;
    [SerializeField] private Sprite mapSign;

    [Header("Signs Size")]
    [SerializeField] private float signSizeX = 1;
    [SerializeField] private float signSizeY = 1;

    [Header("Map Legend")]
    [SerializeField] private GameObject mapLegend;
    [SerializeField] private bool enableLegend = true;
    [SerializeField] private Image startLegendImage;
    [SerializeField] private Image endLegendImage;
    [SerializeField] private Image mapLegendImage;

    [Header("Settings")]
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    [SerializeField] private float roomSizeX = 1;
    [SerializeField] private float roomSizeY = 1;
    [SerializeField] private float spaceBetweenRooms = 1;

    [Header("Setup")]
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject mapCanvas;
    public bool mapEnable { get { return mapPanel.active; } }

    bool isMapBuilded = false;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Map UI Manager in scene!");
        instance = this;
    }

    private void Start()
    {
        HideMap();
        isMapBuilded = false;

        if (startLegendImage != null)
            startLegendImage.sprite = startSign;
        if (endLegendImage != null)
            endLegendImage.sprite = endSign;
        if (mapLegendImage != null)
            mapLegendImage.sprite = mapSign;

        mapLegend.SetActive(enableLegend);
    }

    LevelTemplate level;
    void BuildMap()
    {
        level = LevelBuilder.instance.levelTemplate;

        foreach (var roomPos in level.levelRoomsPositions)
        {
            RoomTemplate room = level.levelRooms[roomPos.x, roomPos.y];

            BuildRoomImage(room);
        }

        isMapBuilded = true;
    }

    void BuildRoomImage(RoomTemplate roomTemplate)
    {
        Image image = Instantiate(new GameObject(), mapCanvas.transform).AddComponent<Image>();

        switch (roomTemplate.roomType)
        {
            case RoomType.Deadlock:
                image.sprite = deadlockRoomMapTile;
                break;
            case RoomType.I_room:
                image.sprite = IRoomMapTile;
                break;
            case RoomType.L_room:
                image.sprite = LRoomMapTile;
                break;
            case RoomType.T_room:
                image.sprite = TRoomMapTile;
                break;
            case RoomType.X_room:
                image.sprite = XRoomMapTile;
                break;
        }

        image.transform.localScale = new Vector3(roomSizeX, roomSizeY, 1);

        SetRoomImagePosition(image, roomTemplate.position);
        RightRotateRoomImage(image, roomTemplate);

        if (roomTemplate == level.startRoom)
            SetSignOnRoomImage(image, startSign);
        if (roomTemplate == level.endRoom)
            SetSignOnRoomImage(image, endSign);
        if (roomTemplate.bonusType == BonusType.Map)
            SetSignOnRoomImage(image, mapSign);
    }

    void SetRoomImagePosition(Image roomImage, Vector2Int position)
    {
        roomImage.transform.localPosition = new Vector3(position.x + xOffset, position.y + yOffset, 0) * spaceBetweenRooms;
    }

    void RightRotateRoomImage(Image roomImage, RoomTemplate roomTemplate)
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

        roomImage.transform.Rotate(roomImage.transform.rotation.x, roomImage.transform.rotation.y, zRotation);
    }

    void SetSignOnRoomImage(Image roomImage, Sprite sign)
    {
        if (sign == null)
        {
            Debug.LogWarning("Cant set sign on room because there are no sign sprite");
            return;
        }

        Image roomSign = Instantiate(new GameObject(), roomImage.transform).AddComponent<Image>();
        roomSign.sprite = sign;
        roomSign.transform.localPosition = new Vector3(0, 0, 5);
        roomSign.transform.localScale = new Vector3(signSizeX, signSizeY, 1);
    }

    public void ShowMap()
    {
        if (!isMapBuilded)
            BuildMap();
        mapPanel.SetActive(true);
    }
        
    public void HideMap()
        => mapPanel.SetActive(false);
}
