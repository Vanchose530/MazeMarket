using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ZombieSpittingBlood : Enemy, IDamagable
{
    [Header("Movement (ZombieSpittingBlood)")]
    public float agressiveSpeed;
    [HideInInspector] public float currentSpeed;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;

    [Header("Shoot")]
    [SerializeField] private float timeShoot;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private float forceBlood;

    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;

    [Header("ZombieSpittingBlood States")]
    [SerializeField] private ZombieSpittingBloodSpawnState spawnState;
    [SerializeField] private ZombieSpittingBloodPassiveState passiveState;
    [SerializeField] private ZombieSpittingBloodPursuitState pursuitState;
    [SerializeField] public ZombieSpittingBloodAttackState attackState;
    [SerializeField] public ZombieSpittingBloodRecoveryState recoveryState;

    [Header("Sound")]
    [SerializeField] private SoundEffect groahSE;
    [SerializeField] private SoundEffect preparingSE;
    [SerializeField] private SoundEffect spitSE;
    public SoundEffect hitSE;
    public ZombieSpittingBloodState currentState { get; private set; }
    public bool attack = false;
    // Start is called before the first frame update
    [HideInInspector] public bool isShoot = false;
    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        if (legsAnimator == null)
            legsAnimator = legs.GetComponent<Animator>();
        

        if (spawnState == null)
            spawnState = statesGameObject.GetComponent<ZombieSpittingBloodSpawnState>();
        if (passiveState == null)
            passiveState = statesGameObject.GetComponent<ZombieSpittingBloodPassiveState>();
        if (pursuitState == null)
            pursuitState = statesGameObject.GetComponent<ZombieSpittingBloodPursuitState>();
        if (attackState == null)
            attackState = statesGameObject.GetComponent<ZombieSpittingBloodAttackState>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<ZombieSpittingBloodRecoveryState>();
    }
    private void Awake()
    {
        health = maxHealth;

        target = null;
        agressive = false;

        SetAnimationSettings();
    }
    private void OnEnable()
    {
        try
        {
            GameEventsManager.instance.player.onPlayerDeath += PlayerDeath;
        }
        catch (System.NullReferenceException)
        {
            Invoke("AddEvents", 0.1f);
        }
    }

    private void AddEvents()
    {
        GameEventsManager.instance.player.onPlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.player.onPlayerDeath -= PlayerDeath;
    }
    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        if (alreadySpawnedOnStart)
            SetState(passiveState);
        else
            SetState(spawnState);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState.isFinished)
        {
            if (agressive && !attack)
            {
                SetState(pursuitState);
            }
            else if (agressive && attack)
            {
                SetState(attackState);
            }
            else
                SetState(passiveState);
        }
        else
        {
            currentState.Run();
        }
        Animate();
        Move();
        RotateBody();
        RotateLegs();
    }
    public void TakeDamage(int damage, Transform attack = null)
    {
        if (spawning)
            return;

        health -= damage;



        if (currentState != attackState) SetState(attackState);
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
        bodyAnimator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
    }
    private void SetAnimationSettings()
    {
        bodyAnimator.SetFloat("Attack Multiplier", 1 / timeShoot);
    }

    protected override void PlayerDeath()
    {
        agressive = false;
        SetState(passiveState);
    }

    public void SetState(ZombieSpittingBloodState state)
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
            currentState.zombieSpittingBlood = this;
            currentState.Init();
        }
        if (state == pursuitState)
            AudioManager.instance.PlaySoundEffect(groahSE, transform.position);
    }

    public override void Attack()
    {
        if (isShoot)
            return;
        StartCoroutine("StartAttack");
    }

    private IEnumerator StartAttack() 
    {
        isShoot = true;
        bodyAnimator.SetTrigger("Shoot");
        movementDirection = Vector2.zero;

        AudioManager.instance.PlaySoundEffect(preparingSE, transform.position);
        yield return new WaitForSeconds(timeShoot);

        //targetOnAim = false;

        Shoot();

        attackState.attackCount--;
        targetOnAim = true;
        isShoot = false;
    }

    private void Shoot()
    {
        Vector3 bloodAngle = attackPoint.eulerAngles;
        GameObject blood = Instantiate(bloodPrefab, attackPoint.position, Quaternion.Euler(bloodAngle));
        Rigidbody2D brb = blood.GetComponent<Rigidbody2D>();
        brb.AddForce(blood.transform.up * forceBlood, ForceMode2D.Impulse);

        AudioManager.instance.PlaySoundEffect(spitSE,transform.position);
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

        yield return new WaitForSeconds(spawningTime);

        SetState(passiveState);


        effect.GetComponent<Animator>().SetFloat("Speed", 2 / spawningTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, spawningTime / 2);

        spawning = false;
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
    private void Move()
    {
        if (isShoot)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = movementDirection * currentSpeed;
    }

}
