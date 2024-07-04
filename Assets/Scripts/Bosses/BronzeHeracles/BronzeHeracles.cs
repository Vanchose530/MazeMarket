using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BronzeHeracles : Enemy, IDamagable
{
    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    //[SerializeField] private Animator legsAnimator;
    [Header("Weapons")]
    [SerializeField] private GameObject stone;
    [SerializeField] private GameObject arrow;
    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;
    [Header("Attack Points")]
    [SerializeField] private Transform archerPoint;
    [SerializeField] private Transform stonePoint;
    [Header("Sound Effects")]
    [SerializeField] private GameObject damageSoundPrefab;
    //[SerializeField] private GameObject punchSoundPrefab;
    //[SerializeField] private GameObject groahSoundPrefab;
    [Header("Bow")]
    [SerializeField] private float timeTakeBow;//время доставания лука
    [SerializeField] private float aimingBow;//время доставания лука
    [SerializeField] private float timeToShootBow;//время стрельбы
    [Header("BronzeHeracles AttackStates")]
    [SerializeField] private ArcheryState archeryState;
    [SerializeField] private MaceAttackState maceAttackState;
    [SerializeField] private StoneThrowState stoneThrowState;

    [Header("BronzeHeracles State")]
    [SerializeField] private BronzeHeraclesAgressiveState agressiveState;
    [SerializeField] private BronzeHeraclesStayState stayState;
    [SerializeField] private BronzeHeraclesRecoveryState recoveryState;

    [HideInInspector] public List<BronzeHeraclesState> attackState=new List<BronzeHeraclesState>();
    
    public BronzeHeraclesState currentState { get; private set; }
    private bool isShootBow = false;
    private bool isBowInHand = false;
    public bool attack { get;  set; }
    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        //if (legsAnimator == null)
        //    legsAnimator = legs.GetComponent<Animator>();

        if (archeryState == null)
            archeryState = statesGameObject.GetComponent<ArcheryState>();
        if (maceAttackState == null)
            maceAttackState = statesGameObject.GetComponent<MaceAttackState>();
        if (stoneThrowState == null)
            stoneThrowState = statesGameObject.GetComponent<StoneThrowState>();
        if (agressiveState == null)
            agressiveState = statesGameObject.GetComponent<BronzeHeraclesAgressiveState>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<BronzeHeraclesRecoveryState>();
        if (stayState == null)
            stayState = statesGameObject.GetComponent<BronzeHeraclesStayState>();
    }
    private void Awake()
    {
        target = null;
        attack = false;
        attackState.Add(archeryState);
        attackState.Add(maceAttackState);
        attackState.Add(stoneThrowState);

        health = maxHealth;
    }
    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        SetState(archeryState);
    }

    
    private void Update()
    {
        currentState.Run();

        
        RotateBody();
        RotateLegs();
        Move();
    }
    public override void Attack()
    {
        currentState.Run();
    }
    public void SetState(BronzeHeraclesState state)
    {
        try
        {
            currentState.Exit();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("For " + gameObject.name + " state setted for first time");
        }
        finally
        {
            currentState = state;
            currentState.bronzeHeracles = this;
            currentState.Init();
        }
    }
    public override void Spawn()
    {
        throw new System.NotImplementedException();
    }

    protected override void PlayerDeath()
    {
        throw new System.NotImplementedException();
    }
    private void RotateBody()
    {
        if (movementDirection == Vector2.zero)
            return;

        float angle;

        if (targetOnAim)
        {
            Vector2 dir = ((Vector2)target.transform.position - rb.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        }

        rb.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    private void RotateLegs()
    {
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

        legs.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    public void TakeDamage(int damage, Transform attack = null)
    {
        if (spawning)
            return;

        health -= damage;
        EffectsManager.instance.PlaySoundEffect(damageSoundPrefab, transform.position, 1f, 0.9f, 1.1f);

        if (attack != null)
        {
            var effect = Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), attack.rotation);
            Destroy(effect, 1f);
        }

        if (health <= 0)
        {
            EnemyDeathEvent();
            Destroy(gameObject);
        }
    }
    private void Move()
    {
        if (isBowInHand)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = movementDirection * speed;
    }
    public void TakeTheBow() 
    {
        StartCoroutine("StartTakeTheBow");
    }
    private IEnumerator StartTakeTheBow() 
    {
        bodyAnimator.SetFloat("GetTheBow Multiplier", 1/timeTakeBow);
        bodyAnimator.Play("GetTheBow");
        yield return new WaitForSeconds(timeTakeBow);
        isBowInHand = true;
    }
    public void ShootBow() 
    {
        if (isShootBow)
            return;
        StartCoroutine("StartShootBow");
    }
    private IEnumerator StartShootBow() 
    {
        isShootBow = true;
        movementDirection = Vector2.zero;
        bodyAnimator.SetTrigger("ShootABow");
        bodyAnimator.SetFloat("AimingABow Multiplier",1/aimingBow);
        bodyAnimator.SetFloat("ShootABow Multiplier",1/timeToShootBow);
        yield return new WaitForSeconds(timeToShootBow + aimingBow);
         
    }
    public void RemoveBow() 
    {
        StartCoroutine("StartRemoveBow");
    }

    private IEnumerator StartRemoveBow() 
    {
        yield return new WaitForSeconds(2f);
    }


    public void MaceAttack() {
    
    }
}
