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
    [SerializeField] private ChestInstaller chestInstaller;
    [SerializeField] private GameObject cultistsDecor;
    [SerializeField] private ShopInstaller shopInstaller;

    [Header("Enemyes on Room")]
    [SerializeField] private EnemyWavesManagerConfigured _enemyWavesManager;
    public EnemyWavesManagerConfigured enemyWavesManager { get { return _enemyWavesManager; } }

    [Header("Portal")]
    [SerializeField] private Portal portal;

    [Header("Camera")]
    [SerializeField] private VirtualCameraTrigger _virtualCameraTrigger;
    public VirtualCameraTrigger virtualCameraTrigger { get { return _virtualCameraTrigger; } }

    [Header("Setup")]
    [SerializeField] private RoomTrigger roomTrigger; // для работы мини карты

    private void Awake()
    {
        if (roomTrigger != null && roomTrigger.room != this)
            roomTrigger.room = this;
    }

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

    private bool _havePortal;
    public bool havePortal
    {
        get { return _havePortal; }
        set
        {
            SetHavePortal(value);
            _havePortal = value;
        }
    }

    void SetHavePortal(bool value)
    {
        if (value)
        {
            if (enemyesOnRoom == EnemyesOnRoom.None)
            {
                roomTrigger.onPlayerEnterRoomFirstTime += ActivatePortal;
            }
            else
            {
                enemyWavesManager.onPlayerPassRoom += ActivatePortal;
            }
        }
        else
        {
            Destroy(portal.gameObject);
        }
    }

    void ActivatePortal() => portal.Activate();

    void SetBonus(BonusType bonusType)
    {
        if (bonusType != BonusType.DemonsBloodFountain)
            Destroy(demonsBloodFountain.gameObject);
        if (bonusType != BonusType.SodaMachine)
            Destroy(sodaMachine.gameObject);
        if (bonusType != BonusType.Map)
            Destroy(map.gameObject);

        if (bonusType != BonusType.Shop)
            Destroy(shopInstaller.gameObject);
        else
            shopInstaller.Install(bonusValue);

        if (bonusType != BonusType.Chest)
        {
            if (cultistsDecor != null)
                Destroy(cultistsDecor.gameObject);

            Destroy(chestInstaller.gameObject);
        }
        else
        {
            chestInstaller.Install(bonusValue);
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

    public RoomType roomType { get; set; }
    public Vector2Int positionInLevel { get; set; }
    public MiniMapRoom miniMapRoom { get; set; }

    public bool playerEnterRoomFirstTime { get; set; }
}
