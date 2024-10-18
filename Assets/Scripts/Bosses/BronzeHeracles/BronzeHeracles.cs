using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BronzeHeracles : Enemy, IDamagable
{
    [Header("Animators")]
    public Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;

    [Header("WeaponsPrefab")]
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject mace;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;

    [Header("Weapons")]
    [SerializeField] private GameObject stonePoint;
    [SerializeField] private GameObject archeryPoint;

    [Header("Sound Effects")]
    [SerializeField] private SoundEffect damageSE;
    [SerializeField] private SoundEffect alivingSE;
    [SerializeField] private SoundEffect aimingBowSE;
    [SerializeField] private SoundEffect shootBowSE;
    [SerializeField] private SoundEffect takeArrowBowSE;
    [SerializeField] private SoundEffect takeMaceSE;
    [SerializeField] private SoundEffect maceAttackSE;
    [SerializeField] private SoundEffect takeStoneSE;
    [SerializeField] private SoundEffect shootStoneSE;

    [Header("Alive")]
    public bool stayOnAwake;
    [HideInInspector] public bool stay;
    public float alivingTime;

    [Header("Bow")]
    [SerializeField] private float timeTakeBow;
    [SerializeField] private float timeNewArrow;
    [SerializeField] private float timeWalkBow;
    [SerializeField] private float timeAimingBow;
    [SerializeField] private float timeToShootBow;
    [SerializeField] private float timeRemoveBow;
    [SerializeField] private int minCountArrow;
    [SerializeField] private int maxCountArrow;
    [SerializeField] private float forceArrow;

    [Header("Mace")]
    [SerializeField] private float timeTakeMace;
    [SerializeField] private float timeAttackMace;
    [SerializeField] private float timeRemoveMace;

    [Header("Stone")]
    [SerializeField] private float timeTakeStone;
    [SerializeField] private float timeWalkStone;
    [SerializeField] private float timeShootStone;
    [SerializeField] private int minCountStone;
    [SerializeField] private int maxCountStone;
    [SerializeField] private float forceStone;



    [Header("BronzeHeracles AttackStates")]
    [SerializeField] private ArcheryState archeryState;
    [SerializeField] private MaceAttackState maceAttackState;
    [SerializeField] private StoneThrowState stoneThrowState;

    [Header("BronzeHeracles State")]
    [SerializeField] private BronzeHeraclesStayState stayState;
    public BronzeHeraclesRecoveryState recoveryState;

    [HideInInspector] public List<BronzeHeraclesState> attackState = new List<BronzeHeraclesState>();
    
    public BronzeHeraclesState currentState { get; private set; }

    [HideInInspector] public bool isShootBow = false;
    [HideInInspector] public bool isRemoveBow = false;
    [HideInInspector] public bool isTakeMace = false;
    [HideInInspector] public bool isAttackMace = false;
    [HideInInspector] public bool isRemoveMace = false;
    [HideInInspector] public bool isTakeStone = false;
    [HideInInspector] public bool isShootStone = false;
    [HideInInspector] public bool isTakeBow = false;
    [HideInInspector] public bool isBowInHand = false;
    [HideInInspector] public bool isWalkStone = false;
    [HideInInspector] public bool isWalkBow = false;
    public bool stand = false;
    private bool aliving;

    public bool attack { get;  set; }


    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        if (legsAnimator == null)
            legsAnimator = legs.GetComponent<Animator>();

        if (archeryState == null)
            archeryState = statesGameObject.GetComponent<ArcheryState>();
        if (maceAttackState == null)
            maceAttackState = statesGameObject.GetComponent<MaceAttackState>();
        if (stoneThrowState == null)
            stoneThrowState = statesGameObject.GetComponent<StoneThrowState>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<BronzeHeraclesRecoveryState>();
        if (stayState == null)
            stayState = statesGameObject.GetComponent<BronzeHeraclesStayState>();
    }
    private void Awake()
    {
        alreadySpawnedOnStart = false;

        target = null;
        attack = false;
        attackState.Add(archeryState);
        attackState.Add(maceAttackState);
        attackState.Add(stoneThrowState);

        health = maxHealth;

        aliving = false;

        mace.GetComponent<Collider2D>().enabled = false;
        
        
        
    }
    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        SetAnimate();

        SetState(stayState);

        
    }

    
    private void Update()
    {
        currentState.Run();

        Animate();
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
        SetState(stayState);
    }
    private void RotateBody()
    {
        if (movementDirection == Vector2.zero && !targetOnAim)
            return;

        float angle;

        if (targetOnAim && !isShootBow)
        {
            Vector2 dir = ((Vector2)target.transform.position - rb.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        else if (targetOnAim && isShootBow)
        {
            Vector2 dir = ((Vector2)target.transform.position - rb.position).normalized;
            angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 50;
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
        if (spawning || aliving)
            return;
        if (alreadySpawnedOnStart) 
            health -= damage;
        AudioManager.instance.PlaySoundEffect(damageSE, transform.position);

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
    private void Animate()
    {
        legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
    }
    private void Move()
    {
        if (stand)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = movementDirection * speed;
    }
    private void SetAnimate() {
        bodyAnimator.SetFloat("AimingShootBow Multiplier", 1 / timeAimingBow);
        bodyAnimator.SetFloat("ShootABow Multiplier", 1 / timeToShootBow);
        bodyAnimator.SetFloat("GetTheBow Multiplier", 1 / timeTakeBow);
        bodyAnimator.SetFloat("RemoveBow Multiplier", 1 / timeRemoveBow);
        bodyAnimator.SetFloat("TakeMace Multiplier", 1 / timeTakeMace);
        bodyAnimator.SetFloat("MaceAttack Multiplier", 1 / timeAttackMace);
        bodyAnimator.SetFloat("RemoveMace Multiplier", 1 / timeRemoveMace);
        bodyAnimator.SetFloat("Alive Multiplier", 1 / alivingTime);
        bodyAnimator.SetFloat("TakeStone Multiplier", 1 / timeTakeStone);
        bodyAnimator.SetFloat("ShootStone Multiplier", 1 / timeShootStone);
        bodyAnimator.SetFloat("WalkStone Multiplier", 1 / timeWalkStone);
        bodyAnimator.SetFloat("NewArrow Multiplier", 1 / timeNewArrow);
    }
    public void LockRigidbody(bool lockMode)
    {
        if (lockMode)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    public void Alive()
    {
        if (!aliving) StartCoroutine("StartAliving");
    }

    private IEnumerator StartAliving()
    {
        aliving = true;

        bodyAnimator.SetTrigger("Alive");

        AudioManager.instance.PlaySoundEffect(alivingSE, transform.position);

        yield return new WaitForSeconds(alivingTime);

        SetState(archeryState);

        aliving = false;

        alreadySpawnedOnStart = true;
    }

    public BronzeHeraclesState RandomState() {

        return attackState[Random.Range(0, attackState.Count)];
    }

    //Методы атак
    public void TakeTheBow()
    {
        if (isTakeBow)
            return;

        StartCoroutine("StartTakeTheBow");
    }
    private IEnumerator StartTakeTheBow() 
    {
        isTakeBow = true;
        stand = true;

        GameObject ar = Instantiate(arrowPrefab, archeryPoint.transform.position, Quaternion.identity);
        ar.transform.SetParent(archeryPoint.transform);
        ar.transform.localRotation = Quaternion.Euler(0, 0, 95); 

        bodyAnimator.SetTrigger("Bow");

        AudioManager.instance.PlaySoundEffect(takeArrowBowSE,transform.position);

        yield return new WaitForSeconds(timeTakeBow);

        isBowInHand = true;
        isTakeBow = false;
        stand = false;
    }
    public void WalkBow() 
    {
        if (isWalkBow)
            return;
        StartCoroutine("StartWalkBow");
    }
    private IEnumerator StartWalkBow() {
        isWalkBow = true;

        yield return new WaitForSeconds(timeWalkBow + timeNewArrow);

        isWalkBow = false;
        stand = true;
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
        bodyAnimator.Play("AimingABow");

        AudioManager.instance.PlaySoundEffect(aimingBowSE, transform.position, timeAimingBow);

        yield return new WaitForSeconds(timeAimingBow);

        // bodyAnimator.SetTrigger("ShootABow");
        AudioManager.instance.PlaySoundEffect(shootBowSE, transform.position, timeToShootBow);

        yield return new WaitForSeconds(timeToShootBow);

        archeryState.countArrow--;

        if (archeryState.countArrow > 0)
        {
            AudioManager.instance.PlaySoundEffect(takeArrowBowSE, transform.position);
            yield return new WaitForSeconds(timeNewArrow);

            GameObject ar = Instantiate(arrowPrefab, archeryPoint.transform.position, Quaternion.identity);
            ar.transform.SetParent(archeryPoint.transform);
            ar.transform.localRotation = Quaternion.Euler(0, 0, 95);
        }
        isShootBow = false;
        stand = false;
    }
    public void ImpulseArrow()
    {
        archeryPoint.transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        archeryPoint.transform.GetChild(0).gameObject.GetComponent<Arrow>().enabled = true;
        archeryPoint.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

        Rigidbody2D arb = archeryPoint.transform.GetChild(0).GetComponent<Rigidbody2D>();
        archeryPoint.transform.GetChild(0).gameObject.transform.SetParent(null);
        arb.AddForce(-(arb.transform.position - Player.instance.transform.position).normalized * forceStone, ForceMode2D.Impulse);
    }
    public void RemoveBow() 
    {
        if (isRemoveBow)
            return;
        StartCoroutine("StartRemoveBow");
    }

    private IEnumerator StartRemoveBow() 
    {
        isRemoveBow = true;

        bodyAnimator.SetTrigger("RemoveBow");

        yield return new WaitForSeconds(timeRemoveBow);

        isBowInHand = false;
        stand = false;

        bodyAnimator.SetTrigger("Default");

        SetState(recoveryState);

        isRemoveBow = false;
    }

    public void TakeMace()
    {
        if (isTakeMace)
            return;

        StartCoroutine("StartTakeMace");
    }
    private IEnumerator StartTakeMace()
    {
        isTakeMace = true;
        stand = true;

        bodyAnimator.SetTrigger("Mace");

        AudioManager.instance.PlaySoundEffect(takeMaceSE, transform.position);

        yield return new WaitForSeconds(timeTakeMace);

        maceAttackState.maceTaken = true;
        stand = false;
        
    }
    public void MaceAttack() 
    {
        if (isAttackMace)
            return;
        StartCoroutine("StartMaceAttack");
    }
    private IEnumerator StartMaceAttack() 
    {
        isAttackMace = true;
        bodyAnimator.Play("MaceAttack");
        mace.GetComponent<Collider2D>().enabled = true;
        AudioManager.instance.PlaySoundEffect(maceAttackSE, transform.position);
        yield return new WaitForSeconds(timeAttackMace);
        mace.GetComponent<Collider2D>().enabled = false;
        attack = false;
    }
    public void RemoveMace()
    {
        if (isRemoveMace)
            return;
        StartCoroutine("StartRemoveMace");
    }
    private IEnumerator StartRemoveMace()
    {
        isRemoveMace = true;
        stand = true;
        yield return new WaitForSeconds(timeRemoveMace);
        bodyAnimator.SetTrigger("Default");
        stand = false;
        SetState(recoveryState);
    }
    public void TakeStone() 
    {
        if (isTakeStone)
            return;


        StartCoroutine("StartTakeStone");
        

    }
    public IEnumerator StartTakeStone() 
    {
        isTakeStone = true;

        GameObject st = Instantiate(stonePrefab, stonePoint.transform.position, Quaternion.identity);
        st.transform.SetParent(stonePoint.transform);
        st.transform.localRotation = Quaternion.Euler(0,0,gameObject.transform.rotation.z);

        bodyAnimator.SetTrigger("Stone");

        AudioManager.instance.PlaySoundEffect(takeStoneSE,transform.position);
        yield return new WaitForSeconds(timeTakeStone);
        isTakeStone = false;
        stoneThrowState.takenStone = true;
    }
    public void WalkStone()
    {
        if (isWalkStone)
            return;

        StartCoroutine("StartWalkStone");
    }
    private IEnumerator StartWalkStone()
    {
        stand = false;
        isWalkStone = true;
        yield return new WaitForSeconds(timeWalkStone);
        isWalkStone = false;
        stand = true;
        stoneThrowState.walkStone = true;
    }
    public void ShootStone()
    {
        if (isShootStone)
            return;


        StartCoroutine("StartShootStone");


    }
    private IEnumerator StartShootStone()
    {
        isShootStone = true;
        movementDirection = Vector2.zero;

        AudioManager.instance.PlaySoundEffect(shootStoneSE, transform.position);

        yield return new WaitForSeconds(timeShootStone);


        stonePoint.transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        stonePoint.transform.GetChild(0).gameObject.GetComponent<Stone>().enabled = true;
        stonePoint.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

        Rigidbody2D srb = stonePoint.transform.GetChild(0).GetComponent<Rigidbody2D>();
        stonePoint.transform.GetChild(0).gameObject.transform.SetParent(null);
        srb.AddForce(-(srb.transform.position - Player.instance.transform.position).normalized * forceStone, ForceMode2D.Impulse);
        

        

        isShootStone = false;
        stoneThrowState.takenStone = false;
        stoneThrowState.walkStone = false;
        stoneThrowState.countStone--;
    }
    public int RandomStone() 
    {
        return Random.Range(minCountStone, maxCountStone);
    }
    public int RandomArrow()
    {
        return Random.Range(minCountArrow, maxCountArrow);
    }
}