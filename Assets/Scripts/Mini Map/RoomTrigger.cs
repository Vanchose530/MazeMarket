using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomTrigger : MonoBehaviour
{
    public Room room { get; set; }

    public event Action onPlayerEnterRoomFirstTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (room.miniMapRoom.status != MiniMapRoomStatus.VisibleWayAndBonus)
                room.miniMapRoom.status = MiniMapRoomStatus.VisibleWayAndBonus;

            if (room.miniMapRoom.playerStatus == MiniMapRoomPlayerStatus.WasNotIn)
            {
                MiniMapUIM.instance.ShowRoomsNear(room);
            }

            PlayerEnterRoom();

            room.miniMapRoom.playerStatus = MiniMapRoomPlayerStatus.NowIn;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            room.miniMapRoom.playerStatus = MiniMapRoomPlayerStatus.WasIn;
        }
    }

    void PlayerEnterRoom()
    {
        if (!room.playerEnterRoomFirstTime)
        {
            onPlayerEnterRoomFirstTime?.Invoke();
            room.playerEnterRoomFirstTime = true;
        }
    }
}
