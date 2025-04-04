using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyWavesTrigger : MonoBehaviour
{
    public EnemyWavesManagerConfigured roomManager { private get; set; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
            roomManager.PlayerEnterRoom();
    }
}
