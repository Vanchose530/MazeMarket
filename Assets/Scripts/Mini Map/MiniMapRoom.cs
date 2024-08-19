using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapRoom : MonoBehaviour
{
    [Header("Player in Room")]
    [SerializeField] private Image coloredPart;
    [SerializeField] private Color playerWasNotInRoomColor;
    [SerializeField] private Color playerWasInRoomColor;

    [Header("Locks")]
    [SerializeField] private Image[] locks;
    public int locksCount { get { return locks.Length; } }
    [SerializeField] private Color redMiasmaColor;
    [SerializeField] private Color mintMiasmaColor;

    [Header("Another")]
    [SerializeField] private Image nonColoredPart;
    private Color nonColoredPartBaseColor;

    private void Awake()
    {
        nonColoredPartBaseColor = nonColoredPart.color;
    }


    // STATUSES

    private MiniMapRoomStatus _status;
    public MiniMapRoomStatus status
    {
        get { return _status; }
        set { _status = value; SetRoomStatusVisualization(value); }
    }
    private void SetRoomStatusVisualization(MiniMapRoomStatus status)
    {
        switch (status)
        {
            case MiniMapRoomStatus.Hidden:
                //coloredPart.color = new Color(0, 0, 0, 0);
                //nonColoredPart.color = new Color(0, 0, 0, 0);
                HideRoom();
                break;
            case MiniMapRoomStatus.VisibleWay:
                nonColoredPart.color = nonColoredPartBaseColor;
                SetRoomPlayerStatusVisualization(playerStatus);
                lockVisible = true;
                break;
            case MiniMapRoomStatus.VisibleWayAndBonus:
                nonColoredPart.color = nonColoredPartBaseColor;
                SetRoomPlayerStatusVisualization(playerStatus);
                lockVisible = true;
                SetRighSignOnRoom();
                break;
        }
    }

    private void SetRighSignOnRoom()
    {
        Sprite sign = null;

        switch (bonusType)
        {
            case BonusType.Chest:
                sign = MiniMapUIM.instance.chestSign;
                break;
            case BonusType.DemonsBloodFountain:
                sign = MiniMapUIM.instance.demonsBloodFountainSign;
                break;
            case BonusType.SodaMachine:
                sign = MiniMapUIM.instance.sodaMachineSign;
                break;
            case BonusType.Shop:
                sign = MiniMapUIM.instance.shopSign;
                break;
            case BonusType.Map:
                sign = MiniMapUIM.instance.mapSign;
                break;
            case BonusType.None:
                if (locks.Length == 1) // если комната является тупиковой
                {
                    RoomTemplate roomTemplate
                        = LevelBuilder.instance.levelTemplate.
                        levelRooms[positionInLevel.x, positionInLevel.y];

                    if (roomTemplate == LevelBuilder.instance.levelTemplate.startRoom)
                    {
                        sign = MiniMapUIM.instance.startSign;
                    }
                    else if (roomTemplate == LevelBuilder.instance.levelTemplate.endRoom)
                    {
                        sign = MiniMapUIM.instance.endSign;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            break;
        }

        MiniMapUIM.instance.SetSignOnMiniMapRoom(this, sign);
    }

    void HideRoom()
    {
        Image[] images = GetComponentsInChildren<Image>();

        foreach (var image in images)
        {
            image.color = new Color(0, 0, 0, 0);
        }
    }

    private MiniMapRoomPlayerStatus _playerStatus;
    public MiniMapRoomPlayerStatus playerStatus
    {
        get { return _playerStatus; }
        set { _playerStatus = value; SetRoomPlayerStatusVisualization(value); }
    }
    private void SetRoomPlayerStatusVisualization(MiniMapRoomPlayerStatus playerStatus)
    {
        if (status == MiniMapRoomStatus.Hidden)
        {
            //coloredPart.color = new Color(0, 0, 0, 0);
            //nonColoredPart.color = new Color(0, 0, 0, 0);
            HideRoom();
            return;
        }

        switch (playerStatus)
        {
            case MiniMapRoomPlayerStatus.WasNotIn:
                coloredPart.color = playerWasNotInRoomColor;
                break;
            case MiniMapRoomPlayerStatus.WasIn:
                coloredPart.color = playerWasInRoomColor;
                break;
            case MiniMapRoomPlayerStatus.NowIn:
                coloredPart.color = playerWasInRoomColor;
                MiniMapUIM.instance.SetPlayerInRoomMark(this);
                break;
        }
    }

    private RoomLockType _lockType;
    public RoomLockType lockType
    {
        get { return _lockType; }
        set { _lockType = value; SetLockVisible(lockVisible); }
    }

    private bool _lockVisible;
    public bool lockVisible
    {
        get { return _lockVisible; }
        set { _lockVisible = value; SetLockVisible(value); }
    }
    private void SetLockVisible(bool locksAreVisible)
    {
        if (!locksAreVisible)
        {
            foreach (var loc in locks)
            {
                loc.color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            switch (lockType)
            {
                case RoomLockType.None:
                    foreach (var loc in locks)
                    {
                        loc.color = new Color(0, 0, 0, 0);
                    }
                    break;
                case RoomLockType.RedMiasmas:
                    foreach (var loc in locks)
                    {
                        loc.color = redMiasmaColor;
                    }
                    break;
                case RoomLockType.MintMiasmas:
                    foreach (var loc in locks)
                    {
                        loc.color = mintMiasmaColor;
                    }
                    break;
            }
        }
    }

    public BonusType bonusType { get; set; }
    public Vector2Int positionInLevel { get; set; }
}
