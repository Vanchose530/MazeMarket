using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemyes Config", menuName = "Scriptable Objects/Configs/Enemyes/Enemyes", order = 1)]
public class EnemyesConfig : ScriptableObject
{
    [Header("Enemyes to next wave")]
    [SerializeField] private int _enemyesToNextWave = 0;
    public int enemyesToNextWave { get { return _enemyesToNextWave; } }

    [Header("Enemy Waves")]
    [SerializeField] private EnemyWaveConfig[] _enemyWaves;
    public EnemyWaveConfig[] enemyWaves { get { return _enemyWaves; } }

    [Header("Reward After Battle")]
    [SerializeField] private RewardAfterBattleConfig _rewardAfterBattle;
    public RewardAfterBattleConfig rewardAfterBattle { get { return _rewardAfterBattle; } }

    public EnemyWaveConfig GetRandomEnemyWave()
    {
        // возвращаем копию конфига с волнами врагов, чтобы не изменять основной конфиг
        return Instantiate(enemyWaves[Random.Range(0, enemyWaves.Length)]);
    }
}
