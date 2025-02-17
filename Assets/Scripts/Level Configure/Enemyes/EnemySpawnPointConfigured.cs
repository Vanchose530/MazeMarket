using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointConfigured : MonoBehaviour
{
    public void SpawnEnemy(Enemy spawningEnemyPrefab)
    {
        spawningEnemyPrefab.alreadySpawnedOnStart = false;

        Enemy enemy = Instantiate(spawningEnemyPrefab, gameObject.transform.position, Quaternion.identity).GetComponent<Enemy>();

        enemy.agressive = true;
    }
}
