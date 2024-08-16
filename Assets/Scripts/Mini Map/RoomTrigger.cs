using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomTrigger : MonoBehaviour
{
    public Room room { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            room.miniMapRoom.status = MiniMapRoomStatus.VisibleWayAndBonus;
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
}
