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
    [SerializeField] private SoundEffect screamSE;

    [Header("Alive")]
    public bool stayOnAwake;
    [HideInInspector] public bool stay;
    public float alivingTime;
    [Header("BronzeHeracles AttackStates")]
    [SerializeField] private BronzeHeraclesState archeryState;
    [SerializeField] private BronzeHeraclesState maceAttackState;
    [SerializeField] private BronzeHeraclesState stoneThrowState;

    [Header("BronzeHeracles State")]
    [SerializeField] private BronzeHeraclesStayState stayState;
    public BronzeHeraclesRecoveryState recoveryState;

    [HideInInspector] public List<BronzeHeraclesState> attackState = new List<BronzeHeraclesState>();

    public BronzeHeraclesState currentState { get; private set; }

    //���������� ��� �������
    public bool isShootBow { get; private set; }
    public bool isRemoveBow { get; private set; }
    public bool isTakeMace { get; private set; }
    public bool isAttackMace { get; private set; }
    public bool isRemoveMace { get; private set; }
    public bool isTakeStone { get; private set; }
    public bool isShootStone { get; private set; }
    public bool isTakeBow { get; private set; }
    public bool isBowInHand { get; private set; }
    public bool isWalkStone { get; private set; }
    public bool isWalkBow { get; private set; }

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
        bodyAnimator.SetFloat("AimingShootBow Multiplier", 1 / archeryState.GetComponent<ArcheryState>().timeAimingBow);
        bodyAnimator.SetFloat("ShootABow Multiplier", 1 / archeryState.GetComponent<ArcheryState>().timeToShootBow);
        bodyAnimator.SetFloat("GetTheBow Multiplier", 1 / archeryState.GetComponent<ArcheryState>().timeTakeBow);
        bodyAnimator.SetFloat("RemoveBow Multiplier", 1 / archeryState.GetComponent<ArcheryState>().timeRemoveBow);
        bodyAnimator.SetFloat("TakeMace Multiplier", 1 / maceAttackState.GetComponent<MaceAttackState>().timeTakeMace);
        bodyAnimator.SetFloat("MaceAttack Multiplier", 1 / maceAttackState.GetComponent<MaceAttackState>().timeAttackMace);
        bodyAnimator.SetFloat("RemoveMace Multiplier", 1 / maceAttackState.GetComponent<MaceAttackState>().timeRemoveMace);
        bodyAnimator.SetFloat("Alive Multiplier", 1 / alivingTime);
        bodyAnimator.SetFloat("TakeStone Multiplier", 1 / stoneThrowState.GetComponent<StoneThrowState>().timeTakeStone);
        bodyAnimator.SetFloat("ShootStone Multiplier", 1 / stoneThrowState.GetComponent<StoneThrowState>().timeShootStone);
        bodyAnimator.SetFloat("WalkStone Multiplier", 1 / stoneThrowState.GetComponent<StoneThrowState>().timeWalkStone);
        bodyAnimator.SetFloat("NewArrow Multiplier", 1 / archeryState.GetComponent<ArcheryState>().timeNewArrow);
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
        if (aliving)
            return;

        StartCoroutine("StartAliving");
    }

    private IEnumerator StartAliving()
    {
        aliving = true;

        bodyAnimator.SetTrigger("Alive");

        var effect = Instantiate(EffectsStorage.instance.bossSpawnEffect, transform.position, transform.rotation);
        effect.GetComponent<Animator>().SetFloat("Speed", 1 / alivingTime);

        yield return new WaitForSeconds(alivingTime);

        effect.GetComponent<Animator>().SetFloat("Speed", 4 / alivingTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, alivingTime / 4);

        bodyAnimator.SetTrigger("Default");

        yield return new WaitForSeconds(0.5f);

        SetState(RandomState());


        aliving = false;

        alreadySpawnedOnStart = true;

        

    }

    public BronzeHeraclesState RandomState() 
    {
        return attackState[Random.Range(0, attackState.Count)];
    }

    //������ ����
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

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeTakeBow);

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

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeWalkBow + archeryState.GetComponent<ArcheryState>().timeNewArrow);

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

        AudioManager.instance.PlaySoundEffect(aimingBowSE, transform.position, archeryState.GetComponent<ArcheryState>().timeAimingBow);

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeAimingBow);

        // bodyAnimator.SetTrigger("ShootABow");
        AudioManager.instance.PlaySoundEffect(shootBowSE, transform.position, archeryState.GetComponent<ArcheryState>().timeToShootBow);

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeToShootBow);

        archeryState.GetComponent<ArcheryState>().countArrow--;

        if (archeryState.GetComponent<ArcheryState>().countArrow > 0)
        {
            AudioManager.instance.PlaySoundEffect(takeArrowBowSE, transform.position);
            yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeNewArrow);

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
        arb.AddForce(-(arb.transform.position - Player.instance.transform.position).normalized * stoneThrowState.GetComponent<StoneThrowState>().forceStone, ForceMode2D.Impulse);
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

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeRemoveBow);

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

        yield return new WaitForSeconds(maceAttackState.GetComponent<MaceAttackState>().timeTakeMace);

        maceAttackState.GetComponent<MaceAttackState>().maceTaken = true;
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
        yield return new WaitForSeconds(maceAttackState.GetComponent<MaceAttackState>().timeAttackMace);
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
        yield return new WaitForSeconds(maceAttackState.GetComponent<MaceAttackState>().timeRemoveMace);
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
        yield return new WaitForSeconds(stoneThrowState.GetComponent<StoneThrowState>().timeTakeStone);
        isTakeStone = false;
        stoneThrowState.GetComponent<StoneThrowState>().takenStone = true;
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
        yield return new WaitForSeconds(stoneThrowState.GetComponent<StoneThrowState>().timeWalkStone);
        isWalkStone = false;
        stand = true;
        stoneThrowState.GetComponent<StoneThrowState>().walkStone = true;
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

        yield return new WaitForSeconds(stoneThrowState.GetComponent<StoneThrowState>().timeShootStone);


        stonePoint.transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        stonePoint.transform.GetChild(0).gameObject.GetComponent<Stone>().enabled = true;
        stonePoint.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

        Rigidbody2D srb = stonePoint.transform.GetChild(0).GetComponent<Rigidbody2D>();
        stonePoint.transform.GetChild(0).gameObject.transform.SetParent(null);
        srb.AddForce(-(srb.transform.position - Player.instance.transform.position).normalized * stoneThrowState.GetComponent<StoneThrowState>().forceStone, ForceMode2D.Impulse);
        

        

        isShootStone = false;
        stoneThrowState.GetComponent<StoneThrowState>().takenStone = false;
        stoneThrowState.GetComponent<StoneThrowState>().walkStone = false;
        stoneThrowState.GetComponent<StoneThrowState>().countStone--;
    }
    public void Scream() 
    {
        AudioManager.instance.PlaySoundEffect(screamSE, transform.position);
    }
    public int RandomStone() 
    {
        return Random.Range(stoneThrowState.GetComponent<StoneThrowState>().minCountStone, stoneThrowState.GetComponent<StoneThrowState>().maxCountStone);
    }
    public int RandomArrow()
    {
        return Random.Range(archeryState.GetComponent<ArcheryState>().minCountArrow, archeryState.GetComponent<ArcheryState>().maxCountArrow);
    }
}