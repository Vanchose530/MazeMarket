using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public RoomManager roomManager { private get; set; }

    private EnemySpawnPoint[] enemySpawnPoints;

    public bool active { get; private set; }

    private int _enemyCount;

    public int enemyCount
    {
        get { return _enemyCount; }
        set { _enemyCount = value; roomManager.UpdateEnemyCount(); }
    }

    private void Awake()
    {
        enemySpawnPoints = GetComponentsInChildren<EnemySpawnPoint>();

        foreach (var point in enemySpawnPoints)
        {
            point.wave = this;
        }
    }

    public void StartWave()
    {
        active = true;
        enemyCount += enemySpawnPoints.Length;

        foreach (var point in enemySpawnPoints)
        {
            var enemy = point.SpawnEnemy();
            enemy.onEnemyDeath +=
                () => enemyCount--;
        }

        roomManager.UpdateEnemyCount();

        Debug.Log("Enemyes in room: " + roomManager.enemyCount);
    }
}
