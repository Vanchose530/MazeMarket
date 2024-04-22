using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject spawningEnemyPrefab;

    public EnemyWave wave { private get; set; }

    private void Start()
    {
        spawningEnemyPrefab.GetComponent<Enemy>().alreadySpawnedOnStart = false;
    }

    public Enemy SpawnEnemy()
    {
        Enemy enemy = Instantiate(spawningEnemyPrefab, gameObject.transform.position, Quaternion.identity).GetComponent<Enemy>();
        Destroy(gameObject, 0.1f);
        return enemy; 
    }
}
