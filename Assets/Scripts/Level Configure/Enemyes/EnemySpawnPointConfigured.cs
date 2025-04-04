using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointConfigured : MonoBehaviour
{
    [HideInInspector] public EnemyWavesManagerConfigured wavesManager;

    public void SpawnEnemy(Enemy spawningEnemyPrefab)
    {
        spawningEnemyPrefab.alreadySpawnedOnStart = false;

        Enemy enemy = Instantiate(spawningEnemyPrefab, gameObject.transform.position, Quaternion.identity).GetComponent<Enemy>();

        wavesManager.IncrementEnemyCount();
        enemy.onEnemyDeath += () => wavesManager.DecrementEnemyCount();

        enemy.agressive = true;
    }
}
