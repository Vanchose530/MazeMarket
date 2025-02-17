using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class EnemyWavesManagerConfigured : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera virtualCamera { get { return _virtualCamera; } }
    [SerializeField] private bool staticCamera;

    [Header("Entering Room")]
    [SerializeField] private LockMiasma[] lockMiasmas;
    [SerializeField] private EnemyWavesTrigger[] triggers;

    [Header("Lock/Unlock Room")]
    [SerializeField] private float lockExitsTime = 1f;
    [SerializeField] private float unlockExitsTime = 1f;

    [Header("Enemye Spawn Points")]
    [SerializeField] private EnemySpawnPointConfigured[] spawnPoints;

    public int wavesCount = 1;
    private int nextWaveIndex;
    private bool lastWave;

    int enemyesToNextWave;

    public int enemyCount { get; private set; }

    private bool roomPassed = false;

    public event Action onPlayerEnterRoom;
    public event Action onPlayerPassRoom;

    [ContextMenu("Check Enemy Spawn Points in children")]
    private void CheckEnemySpawnPointsInChildren()
    {
        spawnPoints = GetComponentsInChildren<EnemySpawnPointConfigured>();
    }

    private void OnEnable()
    {
        onPlayerEnterRoom += CloseAllExits;
        onPlayerEnterRoom += DestroyAllTriggers;
        onPlayerEnterRoom += StartNextWave;
        onPlayerEnterRoom += EnableVirtualCamera;

        onPlayerEnterRoom += SetBattlePlayerCondition;
        onPlayerPassRoom += SetDefaultPlayerCondition;
    }

    private void OnDisable()
    {
        onPlayerEnterRoom -= CloseAllExits;
        onPlayerEnterRoom -= DestroyAllTriggers;
        onPlayerEnterRoom -= StartNextWave;
        onPlayerEnterRoom -= EnableVirtualCamera;

        onPlayerEnterRoom -= SetBattlePlayerCondition;
        onPlayerPassRoom -= SetDefaultPlayerCondition;
    }

    void EnableVirtualCamera()
        => _virtualCamera.enabled = true;
    void SetBattlePlayerCondition()
        => PlayerConditionsManager.instance.currentCondition = PlayerConditions.Battle;
    void SetDefaultPlayerCondition()
        => PlayerConditionsManager.instance.currentCondition = PlayerConditions.Default;

    public void UpdateEnemyCount()
    {
        // ????

        int enemyCountBuffer = 0;

        //foreach (var wave in enemyWaves)
        //{
        //    if (wave.active)
        //        enemyCountBuffer += wave.enemyCount;
        //}

        enemyCount = enemyCountBuffer;

        if (enemyCount <= enemyesToNextWave && !lastWave)
        {
            StartNextWave();
        }
        else if (lastWave && enemyCount == 0)
        {
            PassRoom();
        }
    }

    private void StartNextWave()
    {
        if (wavesCount == 0)
        {
            PassRoom();
            return;
        }

        try
        {
            StartWave(EnemyesConfigurator.instance.GetEnemyWave());
            nextWaveIndex++;

            if (nextWaveIndex >= wavesCount)
            {
                lastWave = true;
                if (enemyCount == 0)
                    PassRoom();
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            lastWave = true;
            if (enemyCount == 0)
                PassRoom();
        }
    }

    void StartWave(EnemyWaveConfig waveConfig)
    {
        int wi = 0;

        ShuffleSpawnPointsArray();

        foreach (var sp in spawnPoints)
        {
            if (wi == waveConfig.enemies.Count)
                break;

            sp.SpawnEnemy(waveConfig.enemies[wi]);
            wi++;
        }

        if (wi < waveConfig.enemies.Count)
        {
            Debug.LogWarning("There are not enough Enemy Spawn Points for the entire Enemy Wave to appear! "
                + gameObject.name);
        }
    }

    void ShuffleSpawnPointsArray()
    {
        for (int i = spawnPoints.Length - 1; i >= 1; i--)
        {
            int j = UnityEngine.Random.Range(0, spawnPoints.Length - 1);
  
            var temp = spawnPoints[j];
            spawnPoints[j] = spawnPoints[i];
            spawnPoints[i] = temp;
        }
    }

    public void PlayerEnterRoom()
    {
        if (wavesCount == 0)
            return;

        if (onPlayerEnterRoom != null && !roomPassed)
            onPlayerEnterRoom();
    }

    private void PassRoom()
    {
        foreach (var miasma in lockMiasmas)
        {
            miasma.Unlock(unlockExitsTime, true);
        }

        if (onPlayerPassRoom != null)
            onPlayerPassRoom();

        roomPassed = true;

        _virtualCamera.enabled = false;

        EnemyesConfigurator.instance.GenerateEnemyWavesBag();
    }

    private void CloseAllExits()
    {
        foreach (var miasma in lockMiasmas)
        {
            miasma.Lock(lockExitsTime);
        }
    }

    private void DestroyAllTriggers()
    {
        foreach (var trigger in triggers)
        {
            Destroy(trigger.gameObject);
        }
    }
}
