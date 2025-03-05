using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RoomLayout : MonoBehaviour
{
    private Vector2Int _position;
    public Vector2Int position
    {
        get { return _position; }
        set
        { 
            _position = value;
        }
    }
    public float distanceToStartingRoom { get; private set; }

    public RoomType roomType { get; private set; }

    private RoomLockType _lockType;

    public RoomLockType lockType
    {
        get { return _lockType; }
        set
        {
            _lockType = value;
            switch (value)
            {
                case RoomLockType.None:
                    lockMiasmasSprite.color = noneMiasmaColor;
                    break;
                case RoomLockType.MintMiasmas:
                    lockMiasmasSprite.color = mintMiasmaColor;
                    break;
                case RoomLockType.RedMiasmas:
                    lockMiasmasSprite.color = redMiasmaColor;
                    break;
            }
        }
    }

    private bool _havePortal;

    public bool havePortal
    {
        get { return _havePortal; }
        set
        {
            if (portalIndicator != null)
                portalIndicator.SetActive(value);
            _havePortal = value;
        }
    }

    [Header("Setup")]
    [SerializeField] private TextMeshProUGUI roomTextTMP;
    [SerializeField] private SpriteRenderer enemyCircle1;
    [SerializeField] private SpriteRenderer enemyCircle2;
    [SerializeField] private SpriteRenderer enemyCircle3;
    [SerializeField] private SpriteRenderer lockMiasmasSprite;
    [SerializeField] private GameObject portalIndicator;
    [SerializeField] private Color noneMiasmaColor;
    [SerializeField] private Color mintMiasmaColor;
    [SerializeField] private Color redMiasmaColor;

    // --- Transitions --- //
    private TransitionLayout _transitionUp;
    private TransitionLayout _transitionRight;
    private TransitionLayout _transitionDown;
    private TransitionLayout _transitionLeft;

    public TransitionLayout transitionUp { get { return _transitionUp; } set { _transitionUp = value; SetRoomType(); } }
    public TransitionLayout transitionRight { get { return _transitionRight; } set { _transitionRight = value; SetRoomType(); } }
    public TransitionLayout transitionDown { get { return _transitionDown; } set { _transitionDown = value; SetRoomType(); } }
    public TransitionLayout transitionLeft { get { return _transitionLeft; } set { _transitionLeft = value; SetRoomType(); } }

    public int transitionsCount
    {
        get
        {
            int transitionsCount = 0;

            if (transitionUp != null)
                transitionsCount++;
            if (transitionRight != null)
                transitionsCount++;
            if (transitionLeft != null)
                transitionsCount++;
            if (transitionDown != null)
                transitionsCount++;

            return transitionsCount;
        }
    }

    [HideInInspector] public bool canMakeTransition = true;


    // --- Bonuses --- //
    private float _bonusValue = 0;
    public float bonusValue
    {
        get { return _bonusValue; }
        set
        {
            if (bonusType == BonusType.Chest/* || true*/) // я тут чёто намудрил
            {
                if (value != 0)
                {
                    roomTextTMP.text = System.Math.Round(value, 2).ToString();
                }
                else
                {
                    roomTextTMP.text = string.Empty;
                }
            }
            _bonusValue = value;
        }
    }
    private BonusType _bonusType;
    public BonusType bonusType
    {
        get { return _bonusType; }
        set
        {
            switch (value)
            {
                case BonusType.Shop:
                    roomTextTMP.text = "Shp";
                    roomTextTMP.color = Color.cyan;
                    break;
                case BonusType.Map:
                    roomTextTMP.text = "Map";
                    roomTextTMP.color = Color.yellow;
                    break;
                case BonusType.DemonsBloodFountain:
                    roomTextTMP.text = "Bld";
                    roomTextTMP.color = Color.blue;
                    break;
                case BonusType.SodaMachine:
                    roomTextTMP.text = "Sda";
                    roomTextTMP.color = Color.red;
                    break;
                case BonusType.Chest:
                    if (bonusValue != 0)
                    {
                        roomTextTMP.text = System.Math.Round(bonusValue, 2).ToString();
                    }
                    else
                    {
                        roomTextTMP.text = string.Empty;
                    }
                break;
                case BonusType.None:
                    roomTextTMP.text = string.Empty;
                    break;
            }
            _bonusType = value;
        }
    }
    [HideInInspector] public bool canSetBonusInRoom = true;

    private EnemyesOnRoom _enemyesOnRoom;

    public EnemyesOnRoom enemyesOnRoom
    {
        get { return _enemyesOnRoom; }
        set
        {
            switch (value)
            {
                case EnemyesOnRoom.None:
                    enemyCircle1.enabled = false;
                    enemyCircle2.enabled = false;
                    enemyCircle3.enabled = false;
                    break;
                case EnemyesOnRoom.OneWave:
                    enemyCircle1.enabled = true;
                    enemyCircle2.enabled = false;
                    enemyCircle3.enabled = false;
                    break;
                case EnemyesOnRoom.TwoWaves:
                    enemyCircle1.enabled = true;
                    enemyCircle2.enabled = true;
                    enemyCircle3.enabled = false;
                    break;
                case EnemyesOnRoom.ThreeWaves:
                    enemyCircle1.enabled = true;
                    enemyCircle2.enabled = true;
                    enemyCircle3.enabled = true;
                    break;
            }
            _enemyesOnRoom = value;
        }
    }

    void SetRoomType()
    {
        switch (transitionsCount)
        {
            case 0:
                roomType = RoomType.None;
                break;
            case 1:
                roomType = RoomType.Deadlock;
                break;
            case 2:
                if ((transitionUp != null && transitionDown != null) || (transitionLeft != null && transitionRight != null))
                {
                    roomType = RoomType.I_room;
                }
                else
                {
                    roomType = RoomType.L_room;
                }
                break;
            case 3:
                roomType = RoomType.T_room;
                break;
            case 4:
                roomType = RoomType.X_room;
                break;
        }
    }
}
