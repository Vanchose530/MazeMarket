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
    [SerializeField] private Map map;
    [SerializeField] private Chest chestDefault;
    [SerializeField] private Chest chestMedium;
    [SerializeField] private Chest chestRare;
    [SerializeField] private GameObject cultistsDecor;
    [SerializeField] private Shop shop;

    [Header("Enemyes on Room")]
    [SerializeField] private EnemyWavesManager _enemyWavesManager;
    public EnemyWavesManager enemyWavesManager { get { return _enemyWavesManager; } }

    [Header("Camera")]
    [SerializeField] private VirtualCameraTrigger _virtualCameraTrigger;
    public VirtualCameraTrigger virtualCameraTrigger { get { return _virtualCameraTrigger; } }

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
        if (bonusType != BonusType.Map)
            Destroy(map.gameObject);
        if (bonusType != BonusType.Shop)
            Destroy(shop.gameObject);

        if (bonusType != BonusType.Chest)
        {
            if (cultistsDecor != null)
                Destroy(cultistsDecor.gameObject);

            Destroy(chestDefault.gameObject);
            Destroy(chestMedium.gameObject);
            Destroy(chestRare.gameObject);
        }
        else
        {
            // в дальнейшем сделать градацию по сундукам разной редкости!

            Destroy(chestDefault.gameObject);
            // Destroy(chestMedium.gameObject);
            Destroy(chestRare.gameObject);
        }
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
