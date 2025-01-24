using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class Zombie : Enemy, IDamagable
{
    [Header("Movement (Zombie)")]
    public float agressiveSpeed;
    [HideInInspector] public float currentSpeed;

    [Header("Attack (Punch)")]
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    float nextAttackTime;
    int punchSide = 1;

    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;

    [Header("Sound Effects")]
    [SerializeField] private GameObject damageSoundPrefab;
    [SerializeField] private GameObject punchSoundPrefab;
    [SerializeField] private GameObject groahSoundPrefab;

    [Header("Zombie States")]
    [SerializeField] private ZombieSpawnState spawnState;
    [SerializeField] private ZombiePassiveState passiveState;
    [SerializeField] private ZombieAgressiveState agressiveState;
    public ZombieState currentState { get; private set; }

    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        if (legsAnimator == null)
            legsAnimator = legs.GetComponent<Animator>();

        if (spawnState == null)
            spawnState = statesGameObject.GetComponent<ZombieSpawnState>();
        if (passiveState == null)
            passiveState = statesGameObject.GetComponent<ZombiePassiveState>();
        if (agressiveState == null)
            agressiveState = statesGameObject.GetComponent<ZombieAgressiveState>();
    }

    private void Awake()
    {
        health = maxHealth; 

        target = null;
        agressive = false;
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
            {
                SetState(agressiveState);
            }
            else
            {
                SetState(passiveState);
            }
        }
        else
        {
            currentState.Run();
        }
        
        Move();
        Animate();
        RotateBody();
        RotateLegs();
        CountTimeVariables();
    }

    public override void Attack()
    {
        if (nextAttackTime > 0) return;

        bodyAnimator.SetTrigger("Attack");

        var soundEffect = Instantiate(punchSoundPrefab);
        soundEffect.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        Destroy(soundEffect, attackCooldown);

        Collider2D[] hitDamagedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (var obj in hitDamagedObjects)
        {
            if (obj.gameObject.CompareTag("Enemy")) continue;

            IDamagable damagedObj = obj.GetComponent<IDamagable>();

            if (damagedObj != null) damagedObj.TakeDamage(damage, transform);

        }

        if (punchSide == 0) { punchSide = 1; }
        else if (punchSide == 1) { punchSide = 0; }

        nextAttackTime = attackCooldown;
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        if (spawning)
            return;

        health -= damage;

        var soundEffect = Instantiate(damageSoundPrefab);
        soundEffect.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        Destroy(soundEffect, 2f);

        if (currentState != agressiveState) SetState(agressiveState); 
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

    private void SetState(ZombieState state)
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
            currentState.zombie = this;
            currentState.Init();
        }

        if (state == agressiveState)
            EffectsManager.instance.PlaySoundEffect(groahSoundPrefab, transform.position, 5f, 0.8f, 1.2f);
    }

    private void Animate()
    {
        bodyAnimator.SetFloat("Punch Side", punchSide);
        bodyAnimator.SetBool("Follow Player", targetOnAim);
        bodyAnimator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void CountTimeVariables()
    {
        if (nextAttackTime > 0) nextAttackTime -= Time.deltaTime;
    }

    private void Move()
    {
        rb.velocity = movementDirection * currentSpeed;
    }

    private void RotateBody()
    {
        if (movementDirection == Vector2.zero)
            return;

        if (targetOnAim)
        {
            Vector2 dir = ((Vector2)target.transform.position - rb.position).normalized;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(legs.transform.eulerAngles.z, targetAngle - 90, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 legsRotation = legs.transform.eulerAngles;
            rb.transform.localRotation = Quaternion.Euler(0, 0, angle);
            legs.transform.eulerAngles = legsRotation;
            // legs.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            Vector3 rotation = legs.transform.eulerAngles;
            rb.transform.localEulerAngles = legs.transform.eulerAngles;
            legs.transform.eulerAngles = rotation;
        }
    }

    private void RotateLegs()
    {
        if (movementDirection == Vector2.zero)
        {
            legs.transform.localRotation = Quaternion.identity;
            return;
        }

        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(legs.transform.eulerAngles.z, targetAngle - 90, ref turnSmoothVelocity, turnSmoothTime);

        legs.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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

        if (agressive)
            SetState(agressiveState);
        else
            SetState(passiveState);

        EffectsManager.instance.PlaySoundEffect(EffectsStorage.instance.enemySpawnSound, 3f, 0.9f, 1.1f);

        effect.GetComponent<Animator>().SetFloat("Speed", 2 / spawningTime); 
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, spawningTime / 2);

        spawning = false;
    }
}
