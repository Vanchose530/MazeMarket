using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemyes Config", menuName = "Scriptable Objects/Configs/Enemyes/Enemyes", order = 1)]
public class EnemyesConfig : ScriptableObject
{
    [Header("Enemyes to next wave")]
    [SerializeField] private int enemyesToNextWave = 0;

    [Header("Enemy Waves")]
    [SerializeField] private EnemyWaveConfig[] _enemyWaves;
    public EnemyWaveConfig[] enemyWaves { get { return _enemyWaves; } }

    public EnemyWaveConfig GetRandomEnemyWave()
    {
        // ���������� ����� ������� � ������� ������, ����� �� �������� �������� ������
        return Instantiate(enemyWaves[Random.Range(0, enemyWaves.Length)]);
    }
}
