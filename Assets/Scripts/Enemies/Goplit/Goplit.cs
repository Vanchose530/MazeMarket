using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;



public class Goplit : Enemy, IDamagable
{
    [Header("Spear")]
    public GameObject spear;
    [Header("Attack")]
    public float speedAttack;
    public float aimingTime;
    public float timeAttack;
    public float timeAttackEnd;
    public float firingRate;
    [Header ("Animators")]
    public Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;
    [Header("Sound effect")]
    [SerializeField] private SoundEffect damageSoundSE;
    [SerializeField] private GameObject alivingEffectSoundPrefab;
    [SerializeField] private GameObject alivingSoundPrefab;
    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject alivingEffect;

    [Header("Statue stay/alive")]
    public bool stayOnAwake;
    [HideInInspector] public bool stay;
    public float alivingTime;
    bool aliving;

    [Header("Goplit States")]
    [SerializeField] private GoplitSpawnState spawnState;
    [SerializeField] private GoplitStayState stayState;
    [SerializeField] private GoplitPassiveState passiveState;
    [SerializeField] private GoplitPursuitState pursuitState;
    [SerializeField] public GoplitRecoveryState recoveryState;
    [SerializeField] private GoplitAttackState attackState;
    public GoplitState currentState { get; private set; }
     public bool isRush;
    public bool attack { get; set; }

    private bool isEndAttack;
    float time;
    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        if (legsAnimator == null)
            legsAnimator = legs.GetComponent<Animator>();

        if (spawnState == null)
            spawnState = statesGameObject.GetComponent<GoplitSpawnState>();
        if (stayState == null)
            stayState = statesGameObject.GetComponent<GoplitStayState>();
        if (passiveState == null)
            passiveState = statesGameObject.GetComponent<GoplitPassiveState>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<GoplitRecoveryState>();
        if (pursuitState == null)
            pursuitState = statesGameObject.GetComponent<GoplitPursuitState>();
        if (attackState == null)
            attackState = statesGameObject.GetComponent<GoplitAttackState>();
    }
    private void Awake()
    {
        health = maxHealth;

        target = null;
        agressive = false;
        attack = false;

        isRush = false;

        aliving = false;

        stay = stayOnAwake;

        SetAnimationSettings();

        
    }
    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        if (!alreadySpawnedOnStart)
            SetState(spawnState);
        else if (stayOnAwake)
            SetState(stayState);
        else
            SetState(passiveState);
    }

    private void Update()
    {
        if (attack && isRush)
        {
            currentState.Run();
        }
        if (currentState.isFinished)
        {
            if (agressive)
                SetState(pursuitState);
            
            else
                SetState(passiveState);
        }
        else if(!currentState.isFinished && !attack)
        {
            currentState.Run();
        }

        Move();
        Animate();
        RotateBody();
        RotateLegs();
    }
    public void SetState(GoplitState state)
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
            currentState.goplit = this;
            currentState.Init();
        }
    }
    private void Animate()
    {
        bodyAnimator.SetFloat("Movement Speed", rb.velocity.sqrMagnitude);
        legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void SetAnimationSettings()
    {
        bodyAnimator.SetFloat("Preparing Multiplier", 1 / aimingTime);
        bodyAnimator.SetFloat("AttackEnd Multiplier", 1 / timeAttackEnd);
    }


    public override void Attack()
    {
        
        time=timeAttack;
        StartAttack();
        SetState(attackState);

    }
    public void StartAttack() 
    {
        StartCoroutine("StartAttackCoroutine");
    }
    public IEnumerator StartAttackCoroutine() 
    {
        movementDirection = Vector2.zero;
        bodyAnimator.SetTrigger("Aiming");
        yield return new WaitForSeconds(aimingTime);
        targetOnAim = false;
        spear.GetComponent<Collider2D>().enabled = true;
        movementDirection = -(rb.position - Player.instance.rb.position);
        isRush = true;
        
    }
    public void Rush() {
        
        time -= Time.deltaTime;
        if (time <= 0)
        {
            isRush = false;
        }
        
        
    }
    public void EndAttack() 
    {
        if (isEndAttack)
            return;
        isRush = false;
        StartCoroutine("EndAttackCoroutine");
    }
    public IEnumerator EndAttackCoroutine() 
    {
        isEndAttack = true;
        spear.GetComponent<Collider2D>().enabled = false;
        bodyAnimator.SetTrigger("AttackEnd");
        yield return new WaitForSeconds(timeAttackEnd);
        bodyAnimator.SetTrigger("Default");
        SetState(recoveryState);
        attack = false;
        isEndAttack = false;
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

        var effect = Instantiate(alivingEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f), new Quaternion(0, 0, 0, 0));
        effect.GetComponent<Animator>().SetFloat("Speed", 1 / alivingTime);

        EffectsManager.instance.PlaySoundEffect(alivingEffectSoundPrefab, rb.position, alivingTime * 1.5f, 0.9f, 1.1f);

        yield return new WaitForSeconds(alivingTime);

        EffectsManager.instance.PlaySoundEffect(alivingSoundPrefab, rb.position, 3f, 0.9f, 1.1f);

        SetState(passiveState);

        effect.GetComponent<Animator>().SetFloat("Speed", 2 / alivingTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, alivingTime);

        aliving = false;
    }

    public override void Spawn()
    {
        StartCoroutine("StartSpawning");
    }
    private IEnumerator StartSpawning()
    {
        spawning = true;

        var effect = Instantiate(EffectsStorage.instance.enemySpawnEffect, transform.position, transform.rotation);
        effect.GetComponent<Animator>().SetFloat("Speed", 1 / spawningTime);

        EffectsManager.instance.PlaySoundEffect(EffectsStorage.instance.enemySpawnEffectSound, spawningTime * 1.5f, 0.9f, 1.1f);

        bodyAnimator.SetFloat("Spawning Time", 1 / spawningTime);
        bodyAnimator.Play("Spawn");

        yield return new WaitForSeconds(spawningTime);

        SetState(passiveState);

        EffectsManager.instance.PlaySoundEffect(EffectsStorage.instance.enemySpawnSound, 3f, 0.9f, 1.1f);

        effect.GetComponent<Animator>().SetFloat("Speed", 2 / spawningTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, spawningTime / 2);

        spawning = false;
    }
    public void TakeDamage(int damage, Transform attack = null)
    {
        if (spawning)
            return;
        AudioManager.instance.PlaySoundEffect(damageSoundSE, rb.position, (1 / firingRate) * 1.5f);
        health -= damage;


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

    protected override void PlayerDeath()
    {
        agressive = false;
        SetState(passiveState);
    }
    private void RotateBody()
    {
        if (movementDirection == Vector2.zero && !targetOnAim)
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
    private void Move()
    {
        if (attack && !isRush)
            rb.velocity = Vector2.zero;
        else if(attack && isRush)
            rb.velocity = movementDirection * speedAttack;
        else
            rb.velocity = movementDirection * speed;
    }
}
