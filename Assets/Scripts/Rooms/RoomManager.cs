using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour, IDataPersistence
{
    [Header("Unique ID")]
    [SerializeField] private string id;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private bool staticCamera;

    [Header("Entering Room")]
    [SerializeField] private LockMiasma[] lockMiasmas;
    [SerializeField] private GlassDoor[] doors; // двери на данный момент не используются в левел дизайне
    [SerializeField] private RoomTrigger[] triggers;

    [Header("Lock/Unlock Room")]
    [SerializeField] private float closeDoorsSpeed = 2f; // двери на данный момент не используются в левел дизайне
    [SerializeField] private float lockExitsTime = 1f;
    [SerializeField] private float unlockExitsTime = 1f;

    [Header("Enemy Waves")]
    [SerializeField] private EnemyWave[] enemyWaves;
    [SerializeField] private int enemyesToNextWave;
    private int nextWaveIndex;
    private bool lastWave;

    public int enemyCount { get; private set; }

    private bool roomPassed = false;

    public event Action onPlayerEnterRoom;
    public event Action onPlayerPassRoom;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnEnable()
    {
        onPlayerEnterRoom += CloseAllExits;
        onPlayerEnterRoom += DestroyAllTriggers;
        onPlayerEnterRoom += StartNextWave;

        onPlayerEnterRoom += () => Player.instance.isOnBattle = true;
        onPlayerPassRoom += () => Player.instance.isOnBattle = false;
    }

    private void OnDisable()
    {
        onPlayerEnterRoom -= CloseAllExits;
        onPlayerEnterRoom -= DestroyAllTriggers;
        onPlayerEnterRoom -= StartNextWave;
    }

    private void OnValidate()
    {
        if (virtualCamera != null && virtualCamera.enabled)
            virtualCamera.enabled = false;
    }

    private void Awake()
    {
        nextWaveIndex = 0;
    }

    private IEnumerator Start()
    {
        foreach (var trirgger in triggers)
        {
            trirgger.roomManager = this;
        }

        foreach (var wave in enemyWaves)
        {
            wave.roomManager = this;
        }

        foreach (var miasma in lockMiasmas)
        {
            miasma.Unlock(time : 0.01f, destroy : false);
        }

        yield return new WaitForSeconds(0.1f);

        if (!staticCamera)
        {
            // virtualCamera.LookAt = Player.instance.followCameraPoint;
            virtualCamera.Follow = Player.instance.followCameraPoint;
        }
        
        virtualCamera.enabled = false;

        if (id == null || id == "")
            Debug.LogError("For Room Manager not setted unique id. Room Manager object: " + gameObject.name);
    }

    public void PlayerEnterRoom()
    {
        if (onPlayerEnterRoom != null && !roomPassed)
            onPlayerEnterRoom();

        virtualCamera.enabled = true;
    }

    public void LoadData(GameData data)
    {
        if (data.passedRoomsId.Contains(id))
            Destroy(gameObject);
            // roomPassed = true;
    }

    public void SaveData(ref GameData data)
    {
        if (!data.passedRoomsId.Contains(id) && roomPassed) 
            data.passedRoomsId.Add(id);
    }

    private void PassRoom()
    {
        foreach (var miasma in lockMiasmas)
        {
            miasma.Unlock(unlockExitsTime, true);
        }

        foreach (var door in doors)
        {
            door.canOpen = true;
        }

        if (onPlayerPassRoom != null)
            onPlayerPassRoom();

        roomPassed = true;

        virtualCamera.enabled = false;
    }

    public void UpdateEnemyCount()
    {
        int enemyCountBuffer = 0;

        foreach (var wave in enemyWaves)
        {
            if (wave.active)
                enemyCountBuffer += wave.enemyCount;
        }

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

    private void CloseAllExits() 
    {
        foreach (var miasma in lockMiasmas)
        {
            miasma.Lock(lockExitsTime);
        }

        foreach (var door in doors) // двери на данный момент не используются в левел дизайне
        {
            door.Close(closeDoorsSpeed, true);
        }
    }

    private void DestroyAllTriggers()
    {
        foreach (var trigger in triggers)
        {
            Destroy(trigger.gameObject);
        }
    }

    private void StartNextWave()
    {
        try
        {
            enemyWaves[nextWaveIndex].StartWave();
            nextWaveIndex++;
        }
        catch (System.IndexOutOfRangeException)
        {
            lastWave = true;
            if (enemyCount == 0)
                PassRoom();
        }
        
    }
}
