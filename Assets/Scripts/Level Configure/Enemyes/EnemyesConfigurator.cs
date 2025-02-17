using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyesConfigurator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private EnemyesConfig enemyesConfig;

    private List<EnemyWaveConfig> enemyWaveBag = new List<EnemyWaveConfig>();

    public static EnemyesConfigurator instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one Enemyes Configurator in scene");
        }
        instance = this;
    }

    public void GenerateEnemyWavesBag()
    {
        enemyWaveBag.Clear();

        foreach (var ew in enemyesConfig.enemyWaves)
        {
            enemyWaveBag.Add(Instantiate(ew));
        }

        for (int i = enemyWaveBag.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, enemyWaveBag.Count - 1);
            
            var temp = enemyWaveBag[j];
            enemyWaveBag[j] = enemyWaveBag[i];
            enemyWaveBag[i] = temp;
        }
    }

    public EnemyWaveConfig GetEnemyWave()
    {
        if (enemyWaveBag.Count == 0)
            GenerateEnemyWavesBag();

        var ew = enemyWaveBag[0];
        enemyWaveBag.Remove(ew);

        return ew;
    }
}
