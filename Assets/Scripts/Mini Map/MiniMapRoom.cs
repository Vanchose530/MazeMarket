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
    [SerializeField] private Color redMiasmaColor;
    [SerializeField] private Color mintMiasmaColor;

    [Header("Another")]
    [SerializeField] private Image nonColoredPart;


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
                break;
            case MiniMapRoomStatus.VisibleWay:
                break;
            case MiniMapRoomStatus.VisibleWayAndBonus:
                break;
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
        switch (playerStatus)
        {
            case MiniMapRoomPlayerStatus.WasNotIn:
                break;
            case MiniMapRoomPlayerStatus.WasIn:
                break;
            case MiniMapRoomPlayerStatus.NowIn:
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
    private void SetLockVisible(bool lockVisible)
    {
        if (lockVisible)
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
}
