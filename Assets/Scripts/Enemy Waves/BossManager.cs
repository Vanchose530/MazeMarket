using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Unique ID")]
    [SerializeField] private string id;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private bool staticCamera;

    [Header("Entering Room")]
    [SerializeField] private LockMiasma[] lockMiasmas;
    [SerializeField] private GlassDoor[] doors; // двери на данный момент не используются в левел дизайне
    [SerializeField] private BossTrigger[] triggers;

    [Header("Lock/Unlock Room")]
    [SerializeField] private float closeDoorsSpeed = 2f; // двери на данный момент не используются в левел дизайне
    [SerializeField] private float lockExitsTime = 1f;
    [SerializeField] private float unlockExitsTime = 1f;

    [SerializeField] private BronzeHeracles bronzeHeracles;

    private bool roomPassed = false;

    public event Action onPlayerEnterRoom;
    public event Action onPlayerPassRoom;
    public static event Action onBossDefeat;

    private void OnEnable()
    {
        onPlayerEnterRoom += CloseAllExits;
        onPlayerEnterRoom += DestroyAllTriggers;
        onPlayerEnterRoom += bronzeHeracles.Alive;
        onBossDefeat += BossDefeat;

        bronzeHeracles.onEnemyDeath += BossDefeat;

        onPlayerEnterRoom += () => PlayerConditionsManager.instance.currentCondition = PlayerConditions.Battle;
        onPlayerPassRoom += () => PlayerConditionsManager.instance.currentCondition = PlayerConditions.Default;
    }

    private void OnDisable()
    {
        onPlayerEnterRoom -= CloseAllExits;
        onPlayerEnterRoom -= DestroyAllTriggers;
        onPlayerEnterRoom -= bronzeHeracles.Alive;
        onBossDefeat -= BossDefeat;

        bronzeHeracles.onEnemyDeath -= BossDefeat;
    }

    private void OnValidate()
    {
        if (virtualCamera != null && virtualCamera.enabled)
            virtualCamera.enabled = false;
    }

    private IEnumerator Start()
    {
        foreach (var trirgger in triggers)
        {
            trirgger.bossManager = this;
        }
        foreach (var miasma in lockMiasmas)
        {
            miasma.Unlock(time: 0.01f, destroy: false);
        }

        yield return new WaitForSeconds(0.1f);

        if (!staticCamera)
        {
            // virtualCamera.LookAt = Player.instance.followCameraPoint;
            virtualCamera.Follow = Player.instance.followCameraPoint;
           
        }

        virtualCamera.enabled = false;
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

    private void DestroyAllTriggers()
    {
        foreach (var trigger in triggers)
        {
            Destroy(trigger.gameObject);
        }
    }
    public void BossDefeat()
    {
        PassRoom();
    }

}

