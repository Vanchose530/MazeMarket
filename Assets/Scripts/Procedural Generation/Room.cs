using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Lock")]
    [SerializeField] private List<RedMiasma> redMiasmas;
    [SerializeField] private List<GameObject> mintMiasmas;

    [Header("Bonuses")]
    [SerializeField] private DemonsBloodFountain demonsBloodFountain;
    [SerializeField] private VendingMachine sodaMachine;
    // сундук
    // магазин
    // карта

    [Header("Enemyes on Room")]
    [SerializeField] private EnemyWavesManager _enemyWavesManager;
    public EnemyWavesManager enemyWavesManager { get { return _enemyWavesManager; } }

    public float bonusValue { get; set; }
    BonusType _bonusType;
    public BonusType bonusType
    {
        get { return _bonusType; }
        set
        {
            SetBonus(value);
            _bonusType = value;
        }
    }

    EnemyesOnRoom _enemyesOnRoom;
    public EnemyesOnRoom enemyesOnRoom
    {
        get { return _enemyesOnRoom; }
        set
        {
            SetEnemyesOnRoom(value);
            _enemyesOnRoom = value;
        }
    }

    RoomLockType _lockType;
    public RoomLockType lockType
    {
        get { return _lockType; }
        set
        {
            SetRoomLock(value);
            _lockType = value;
        }
    }

    void SetBonus(BonusType bonusType)
    {
        if (bonusType != BonusType.DemonsBloodFountain)
            Destroy(demonsBloodFountain.gameObject);
        if (bonusType != BonusType.SodaMachine)
            Destroy(sodaMachine.gameObject);
    }

    void SetEnemyesOnRoom(EnemyesOnRoom enemyesOnRoom)
    {
        try
        {
            switch (enemyesOnRoom)
            {
                case EnemyesOnRoom.None:
                    Destroy(_enemyWavesManager.gameObject);
                    break;
                case EnemyesOnRoom.OneWave:
                    _enemyWavesManager.wavesCount = 1;
                    break;
                case EnemyesOnRoom.TwoWaves:
                    _enemyWavesManager.wavesCount = 2;
                    break;
                case EnemyesOnRoom.ThreeWaves:
                    _enemyWavesManager.wavesCount = 3;
                    break;
            }
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Room dont have Enemy Waves Manager to set enemyes on room!");
        }
    }

    void SetRoomLock(RoomLockType roomLockType)
    {
        if (roomLockType != RoomLockType.MintMiasmas)
        {
            foreach (var miasma in mintMiasmas)
            {
                Destroy(miasma);
            }
        }
        if (roomLockType != RoomLockType.RedMiasmas)
        {
            foreach (var miasma in redMiasmas)
            {
                Destroy(miasma.gameObject);
            }
        }
    }
}
