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

    [ContextMenu("Check All in children")]
    private void AllChecks()
    {
        CheckEnemySpawnPointsInChildren();
        CheckLockMiasmasInChildren();
        CheckEnemyWavesTriggerInChildren();
    }

    [ContextMenu("Check Enemy Spawn Points in children")]
    private void CheckEnemySpawnPointsInChildren()
    {
        spawnPoints = null;
        spawnPoints = GetComponentsInChildren<EnemySpawnPointConfigured>();
    }

    [ContextMenu("Check Lock Miasmas in children")]
    private void CheckLockMiasmasInChildren()
    {
        lockMiasmas = null;
        lockMiasmas = GetComponentsInChildren<LockMiasma>();
    }

    [ContextMenu("Check Enemy Waves Trigger in children")]
    private void CheckEnemyWavesTriggerInChildren()
    {
        triggers = null;
        triggers = GetComponentsInChildren<EnemyWavesTrigger>();
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

    private IEnumerator Start()
    {
        if (EnemyesConfigurator.instance == null)
            Debug.LogError("There isn't Enemyes Configurator in scene to configure enemyes");

        foreach (var esp in spawnPoints)
        {
            esp.wavesManager = this;
        }

        foreach (var trig in triggers)
        {
            trig.roomManager = this;
        }

        yield return new WaitForSeconds(0.1f);

        if (!staticCamera)
        {
            // virtualCamera.LookAt = Player.instance.followCameraPoint;
            _virtualCamera.Follow = Player.instance.followCameraPoint;
        }

        _virtualCamera.enabled = false;
    }

    void EnableVirtualCamera()
        => _virtualCamera.enabled = true;
    void SetBattlePlayerCondition()
        => PlayerConditionsManager.instance.currentCondition = PlayerConditions.Battle;
    void SetDefaultPlayerCondition()
        => PlayerConditionsManager.instance.currentCondition = PlayerConditions.Default;

    public void DecrementEnemyCount()
    {
        enemyCount--;
        CheckEnemyCount();
    }

    public void IncrementEnemyCount()
        => enemyCount++;

    void CheckEnemyCount()
    {
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
