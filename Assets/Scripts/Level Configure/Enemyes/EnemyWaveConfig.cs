using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemye Wave Config", menuName = "Scriptable Objects/Configs/Enemyes/Wave", order = 2)]
public class EnemyWaveConfig : ScriptableObject
{
    [Header("Enemyes")]
    [SerializeField] private List<Enemy> _enemies;
    public List<Enemy> enemies { get { return _enemies; } }

    public Enemy GetRandomEnemyPrefab()
    {
        if (enemies.Count > 0)
        {
            var en = enemies[Random.Range(0, enemies.Count)];
            enemies.Remove(en);
            return en;
        }
        else
        {
            Debug.LogError("No more enemyes in Enemy Wave Config!");
            return null;
        }
    }
}
