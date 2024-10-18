using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Racine : Enemy, IDamagable
{
    [Header("Root Attack")]
    [SerializeField] private float preparingTime;
    [SerializeField] private float timeToRootUp;
    [SerializeField] private float rootForwardSpeed;
    [SerializeField] private float rootStayTime;
    [SerializeField] private float rootBackSpeed;

    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;

    [Header("Sound Effects")]
    [SerializeField] private GameObject damageSoundPrefab;
    [SerializeField] private GameObject rootSoundPrefab;

    [Header("Racine States")]
    [SerializeField] private RacineSpawnState spawnState;
    [SerializeField] private RacinePassiveState passiveState;
    [SerializeField] private RacineAgressiveState agressiveState;
    [SerializeField] private RacineRecoveryState recoveryState;
    public RacineState currentState { get; private set; }

    public bool attack { get; private set; }

    [Header("Components")]
    [SerializeField] private RacineRoot root;

    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        if (legsAnimator == null)
            legsAnimator = legs.GetComponent<Animator>();
        if (root == null)
            root = attackPoint.gameObject.GetComponent<RacineRoot>();

        if (passiveState == null)
            passiveState = statesGameObject.GetComponent<RacinePassiveState>();
        if (agressiveState == null)
            agressiveState = statesGameObject.GetComponent<RacineAgressiveState>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<RacineRecoveryState>();
        if (spawnState == null)
            spawnState = statesGameObject.GetComponent<RacineSpawnState>();
    }

    private void Awake()
    {
        health = maxHealth;

        target = null;
        agressive = false;
        attack = false;

        root.damage = damage;

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

    private void Update()
    {

        if (currentState.isFinished)
        {
            if (agressive)
                SetState(agressiveState);
            else
                SetState(passiveState);
        }
        else
        {
            currentState.Run();
        }
        
        Move();
        Animate();
        RotateBody();
        RotateLegs();
    }

    private void SetState(RacineState state)
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
            currentState.racine = this;
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
        bodyAnimator.SetFloat("Preparing Multiplier", 1/preparingTime);
        bodyAnimator.SetFloat("Attack Multiplier", 1/timeToRootUp);
    }

    public override void Attack()
    {
        if (attack)
            return;

        StartCoroutine("StartAttack");
    }

    private IEnumerator StartAttack()
    {
        attack = true;
        bodyAnimator.SetTrigger("Attack");
        movementDirection = Vector2.zero;
        yield return new WaitForSeconds(preparingTime + timeToRootUp);

        targetOnAim = false;
        root.RootForward(rootForwardSpeed);
        EffectsManager.instance.PlaySoundEffect(rootSoundPrefab, transform.position, 1 / rootForwardSpeed, 0.7f, 1.2f);

        yield return new WaitForSeconds(1 / rootForwardSpeed + rootStayTime);

        root.RootBack(rootBackSpeed);
        EffectsManager.instance.PlaySoundEffect(rootSoundPrefab, transform.position, 1 / rootBackSpeed, 0.7f, 1.2f);

        yield return new WaitForSeconds(1 / rootBackSpeed);

        root.DisableRoot();

        SetState(recoveryState);
        attack = false;
        bodyAnimator.SetTrigger("Default");
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

    protected override void PlayerDeath()
    {
        agressive = false;
        SetState(passiveState);
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
        if (attack)
            rb.velocity = Vector2.zero; 
        else
            rb.velocity = movementDirection * speed;
    }
}
