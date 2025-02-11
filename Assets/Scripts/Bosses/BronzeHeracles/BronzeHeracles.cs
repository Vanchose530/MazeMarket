using Pathfinding;
using SpriteGlow;
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
    [SerializeField] private SoundEffect pentogramSE;

    [Header("Alive")]
    public bool stayOnAwake;
    [SerializeField] private float pentogramTime;
    [HideInInspector] public bool stay;
    public float alivingTime;

    [Header("Cooldown")]
    [SerializeField] private float firstPhaseHP;
    [SerializeField] private float secondPhaseHP;
    [SerializeField] private float thirdPhaseHP;
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [HideInInspector] public bool isCoolDown { get; private set; }
    [HideInInspector] public bool isDeath { get; private set; }
    private bool invulnerability = true;
    [HideInInspector] public bool isFirstAttack = true;
    [HideInInspector] public int stage = 1;
    [Header("BossManager")]
    public BossManager bossManager;
    public int numberGoplit = 0;
    [Header("BronzeHeracles AttackStates")]
    [SerializeField] private BronzeHeraclesState archeryState;
    [SerializeField] private BronzeHeraclesState maceAttackState;
    [SerializeField] private BronzeHeraclesState stoneThrowState;
    [SerializeField] private BronzeHeraclesState callOfGoplitsState;

    [Header("BronzeHeracles State")]
    [SerializeField] private BronzeHeraclesStayState stayState;
    public BronzeHeraclesRecoveryState recoveryState;
    [SerializeField] private BronzeHeraclesCooldownState cooldownState;
    [SerializeField] private BronzeHeraclesDeathState deathState;

    [HideInInspector] public List<BronzeHeraclesState> attackState = new List<BronzeHeraclesState>();
    
    public BronzeHeraclesState currentState { get; private set; }
    //Корутины
    public Coroutine shootBow;
    public Coroutine removeBow;
    public Coroutine takeMace;
    public Coroutine attackMace;
    public Coroutine removeMace;
    public Coroutine takeStone;
    public Coroutine shootStone;
    public Coroutine takeBow;
    public Coroutine walkStone;
    public Coroutine walkBow;
    public Coroutine coolDownScream;
    public Coroutine deathCoroutine;
    public Coroutine callOfGoplits;
    public Coroutine recoverAttack;
    //Переменные для корутин
    public bool isShootBow => shootBow != null;
    public bool isRemoveBow => removeBow != null;
    public bool isTakeMace => takeMace != null;
    public bool isAttackMace => attackMace != null;
    public bool isRemoveMace => removeMace != null;
    public bool isTakeStone => takeStone != null;
    public bool isShootStone => shootStone != null;
    public bool isTakeBow => takeBow != null;
    public bool isWalkStone => walkStone != null;
    public bool isWalkBow => walkBow != null;
    public bool isCoolDownScream => coolDownScream != null;
    public bool isDeathCoroutine => deathCoroutine != null;
    public bool isCallOfGoplits => callOfGoplits != null;
    public bool isRecoverAttack => recoverAttack != null;
    public bool isBowInHand { get; private set; }
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
        if (interactSpriteGlow == null)
            interactSpriteGlow = GetComponent<SpriteGlowEffect>();

        if (archeryState == null)
            archeryState = statesGameObject.GetComponent<ArcheryState>();
        if (maceAttackState == null)
            maceAttackState = statesGameObject.GetComponent<MaceAttackState>();
        if (stoneThrowState == null)
            stoneThrowState = statesGameObject.GetComponent<StoneThrowState>();
        if (callOfGoplitsState == null)
            callOfGoplitsState = statesGameObject.GetComponent<CallOfGoplits>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<BronzeHeraclesRecoveryState>();
        if (stayState == null)
            stayState = statesGameObject.GetComponent<BronzeHeraclesStayState>();
        if (cooldownState == null)
            cooldownState = statesGameObject.GetComponent<BronzeHeraclesCooldownState>();
        if (deathState == null)
            deathState = statesGameObject.GetComponent<BronzeHeraclesDeathState>();
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
        invulnerability = false;

        mace.GetComponent<Collider2D>().enabled = false;

        interactSpriteGlow.enabled = false;
        legs.GetComponent<SpriteGlowEffect>().enabled = false;

    }
    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        SetAnimate();

        SetState(stayState);

        firstPhaseHP = maxHealth * 0.66f;
        secondPhaseHP = maxHealth * 0.33f;
        thirdPhaseHP = 0f;


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
        if (invulnerability)
            return;
        if (alreadySpawnedOnStart) 
            health -= damage;
        AudioManager.instance.PlaySoundEffect(damageSE, transform.position);



        if (attack != null)
        {
            var effect = Instantiate(damageEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), attack.rotation);
            Destroy(effect, 1f);
        }
        if (health <= firstPhaseHP && stage == 1)
        {
            isCoolDown = true;
            invulnerability = true;
            NextStage();
            SetState(cooldownState);
        }
        if (health <= secondPhaseHP && stage == 2)
        {
            isCoolDown = true;
            invulnerability = true;
            NextStage();
            attackState.Add(callOfGoplitsState);
            SetState(cooldownState);
        }
        if (health <= thirdPhaseHP && stage == 3)
        {
            isDeath = true;
            invulnerability = true;
            NextStage();
            SetState(deathState);
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
        bodyAnimator.SetFloat("Cooldown Multiplier", 1 / cooldownState.cooldownTime);
        bodyAnimator.SetFloat("Death Multiplier", 1 / deathState.deathTime);
        bodyAnimator.SetFloat("CallOfGoplits Multiplier", 1 / callOfGoplitsState.GetComponent<CallOfGoplits>().callGoplitsTime);
        bodyAnimator.SetFloat("RecoverAttack Multiplier", 1 / recoveryState.recoverAttackTime);
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
        invulnerability = true;
        bodyAnimator.Play("Alive");

        var effect = Instantiate(EffectsStorage.instance.bossSpawnEffect, transform.position, transform.rotation);
        effect.GetComponent<Animator>().SetFloat("Speed", 1 / pentogramTime);
        AudioManager.instance.PlaySoundEffect(pentogramSE, transform.position);

        yield return new WaitForSeconds(alivingTime);
        
        effect.GetComponent<Animator>().SetFloat("Speed", 4 / pentogramTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, pentogramTime / 4);

        bodyAnimator.SetTrigger("Default");

        yield return new WaitForSeconds(0.5f);

        SetState(recoveryState);


        aliving = false;
        invulnerability = false;

        alreadySpawnedOnStart = true;

        

    }

    public BronzeHeraclesState RandomState() 
    {
        return attackState[Random.Range(0, attackState.Count)];
    }

    //Методы атак
    public void TakeTheBow()
    {
        takeBow = StartCoroutine(StartTakeTheBow());
    }
    private IEnumerator StartTakeTheBow() 
    {
        stand = true;

        GameObject ar = Instantiate(arrowPrefab, archeryPoint.transform.position, Quaternion.identity);
        ar.transform.SetParent(archeryPoint.transform);
        ar.transform.localRotation = Quaternion.Euler(0, 0, 95); 

        bodyAnimator.SetTrigger("Bow");

        AudioManager.instance.PlaySoundEffect(takeArrowBowSE,transform.position);

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeTakeBow);

        isBowInHand = true;
        stand = false;
    }
    public void WalkBow() 
    {
        walkBow = StartCoroutine(StartWalkBow());
    }
    private IEnumerator StartWalkBow() {

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeWalkBow + archeryState.GetComponent<ArcheryState>().timeNewArrow);

        stand = true;
    }
    public void ShootBow() 
    {
        shootBow = StartCoroutine(StartShootBow());
    }
    private IEnumerator StartShootBow() 
    {

        movementDirection = Vector2.zero;
        bodyAnimator.Play("AimingABow");

        AudioManager.instance.PlaySoundEffect(aimingBowSE, transform.position);

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeAimingBow);

        // bodyAnimator.SetTrigger("ShootABow");
        AudioManager.instance.PlaySoundEffect(shootBowSE, transform.position);

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
        removeBow = StartCoroutine(StartRemoveBow());
    }

    private IEnumerator StartRemoveBow() 
    {

        bodyAnimator.SetTrigger("RemoveBow");

        yield return new WaitForSeconds(archeryState.GetComponent<ArcheryState>().timeRemoveBow);

        isBowInHand = false;
        stand = false;

        bodyAnimator.SetTrigger("Default");

        if (stage == 3 && isFirstAttack)
        {
            isFirstAttack = false;
            SetState(RandomState());
        }
        else
        {
            isFirstAttack = true;
            SetState(recoveryState);
        }

    }

    public void TakeMace()
    {

        takeMace = StartCoroutine(StartTakeMace());
    }
    private IEnumerator StartTakeMace()
    {
        stand = true;

        bodyAnimator.SetTrigger("Mace");

        AudioManager.instance.PlaySoundEffect(takeMaceSE, transform.position);

        yield return new WaitForSeconds(maceAttackState.GetComponent<MaceAttackState>().timeTakeMace);

        maceAttackState.GetComponent<MaceAttackState>().maceTaken = true;
        stand = false;
        
    }
    public void MaceAttack() 
    {
       attackMace = StartCoroutine(StartMaceAttack());
    }
    private IEnumerator StartMaceAttack() 
    {
        bodyAnimator.Play("MaceAttack");
        mace.GetComponent<Collider2D>().enabled = true;
        AudioManager.instance.PlaySoundEffect(maceAttackSE, transform.position);
        yield return new WaitForSeconds(maceAttackState.GetComponent<MaceAttackState>().timeAttackMace);
        mace.GetComponent<Collider2D>().enabled = false;
        attack = false;
    }
    public void RemoveMace()
    {
       removeMace = StartCoroutine(StartRemoveMace());
    }
    private IEnumerator StartRemoveMace()
    {
        stand = true;
        yield return new WaitForSeconds(maceAttackState.GetComponent<MaceAttackState>().timeRemoveMace);
        bodyAnimator.SetTrigger("Default");
        stand = false;

        if (stage == 3 && isFirstAttack)
        {
            isFirstAttack = false;
            SetState(RandomState());
        }
        else
        {
            isFirstAttack = true;
            SetState(recoveryState);
        }
    }
    public void TakeStone() 
    {

       takeStone = StartCoroutine(StartTakeStone());

    }
    public IEnumerator StartTakeStone() 
    {

        GameObject st = Instantiate(stonePrefab, stonePoint.transform.position, Quaternion.identity);
        st.transform.SetParent(stonePoint.transform);
        st.transform.localRotation = Quaternion.Euler(0,0,gameObject.transform.rotation.z);

        bodyAnimator.SetTrigger("Stone");

        AudioManager.instance.PlaySoundEffect(takeStoneSE,transform.position);
        yield return new WaitForSeconds(stoneThrowState.GetComponent<StoneThrowState>().timeTakeStone);
        stoneThrowState.GetComponent<StoneThrowState>().takenStone = true;
    }
    public void WalkStone()
    {
       walkStone = StartCoroutine(StartWalkStone());
    }
    private IEnumerator StartWalkStone()
    {
        stand = false;
        yield return new WaitForSeconds(stoneThrowState.GetComponent<StoneThrowState>().timeWalkStone);
        stand = true;
        stoneThrowState.GetComponent<StoneThrowState>().walkStone = true;
    }
    public void ShootStone()
    {
        shootStone = StartCoroutine(StartShootStone());
    }
    private IEnumerator StartShootStone()
    {
        movementDirection = Vector2.zero;

        AudioManager.instance.PlaySoundEffect(shootStoneSE, transform.position);

        yield return new WaitForSeconds(stoneThrowState.GetComponent<StoneThrowState>().timeShootStone);


        stonePoint.transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
        stonePoint.transform.GetChild(0).gameObject.GetComponent<Stone>().enabled = true;
        stonePoint.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;

        Rigidbody2D srb = stonePoint.transform.GetChild(0).GetComponent<Rigidbody2D>();
        stonePoint.transform.GetChild(0).gameObject.transform.SetParent(null);
        srb.AddForce(-(srb.transform.position - Player.instance.transform.position).normalized * stoneThrowState.GetComponent<StoneThrowState>().forceStone, ForceMode2D.Impulse);
        

        

        stoneThrowState.GetComponent<StoneThrowState>().takenStone = false;
        stoneThrowState.GetComponent<StoneThrowState>().walkStone = false;
        stoneThrowState.GetComponent<StoneThrowState>().countStone--;
    }
    public void CallOfGoplits() 
    {
       callOfGoplits = StartCoroutine(CallOfGoplitsCoroutine());
    }
    private IEnumerator CallOfGoplitsCoroutine() {
        movementDirection = Vector2.zero;
        bodyAnimator.SetTrigger("CallOfGoplits");

        yield return new WaitForSeconds(callOfGoplitsState.GetComponent<CallOfGoplits>().callGoplitsTime);

        bossManager.AliveGoplists();

        if (numberGoplit > bossManager.goplitsList.Count) 
        {
            attackState.Remove(callOfGoplitsState);
        }

        bodyAnimator.SetTrigger("Default");

        if (stage == 3 && isFirstAttack)
        {
            isFirstAttack = false;
            SetState(RandomState());
        }
        else
        {
            isFirstAttack = true;
            SetState(recoveryState);
        }
    }
    public void Scream() 
    {
        AudioManager.instance.PlaySoundEffect(screamSE, transform.position);
    }
    public void CoolDownScream()
    {

       coolDownScream = StartCoroutine(CoolDownCouroitine());
    }
    private IEnumerator CoolDownCouroitine() {
        stand = true;
        movementDirection = Vector2.zero;
        targetOnAim = true;
        interactSpriteGlow.enabled = true;
        

        bodyAnimator.Play("Cooldown");
        yield return new WaitForSeconds(cooldownState.cooldownTime);

        bodyAnimator.SetTrigger("Default");
        stand = false;
        isCoolDown = false;
        invulnerability = false;
        interactSpriteGlow.enabled = false;
        
        ResetCoroutine();
    
    }
    public void RecoverAttack() 
    {
       recoverAttack = StartCoroutine(RecoverAttackCoroutine());
    }
    private IEnumerator RecoverAttackCoroutine() {
        stand = true;
        movementDirection = Vector2.zero;

        bodyAnimator.Play("RecoverAttack");

        yield return new WaitForSeconds(recoveryState.recoverAttackTime);

        bodyAnimator.SetTrigger("Default");
        stand = false;
        targetOnAim = false;
    }
    public void Death()
    {
       deathCoroutine = StartCoroutine(DeathCoroutine());
    }
    private IEnumerator DeathCoroutine() 
    {
        targetOnAim = false;
        stand = true;
        bodyAnimator.Play("Death");

        yield return new WaitForSeconds(deathState.deathTime);

        Destroy(gameObject);
        isDeath = false;
    }
    public int RandomStone() 
    {
        return Random.Range(stoneThrowState.GetComponent<StoneThrowState>().minCountStone, stoneThrowState.GetComponent<StoneThrowState>().maxCountStone);
    }
    public int RandomArrow()
    {
        return Random.Range(archeryState.GetComponent<ArcheryState>().minCountArrow, archeryState.GetComponent<ArcheryState>().maxCountArrow);
    }
    public void NextStage() 
    {
        stage++;
    }
    public void ResetCoroutine()
    {
        shootBow = null;
        attackMace = null;
        removeBow = null;
        takeMace = null;
        removeMace = null;
        takeStone = null;
        shootStone = null;
        takeBow = null;
        isBowInHand = false;
        walkStone = null;
        walkBow = null;
        coolDownScream = null;
        deathCoroutine = null;
        callOfGoplits = null;
        recoverAttack = null;
    }
}