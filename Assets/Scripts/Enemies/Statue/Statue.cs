using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : Enemy, IDamagable
{
    [Header("Attack (Shoot)")]
    [SerializeField] private float bulletForce;
    [SerializeField] private float firingRate;
    [SerializeField] private float spread;
    [SerializeField] private float aimingTurnSmoothTime = 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    float nextAttackTime;

    [Header("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject bulletsAftereffects;
    [SerializeField] private GameObject alivingEffect;

    [Header("Sound Effects")]
    [SerializeField] private GameObject damageSoundPrefab;
    [SerializeField] private SoundEffect shootSE;
    [SerializeField] private GameObject alivingEffectSoundPrefab;
    [SerializeField] private GameObject alivingSoundPrefab;

    [Header("Statue stay/alive")]
    public bool stayOnAwake;
    [HideInInspector] public bool stay;
    public float alivingTime;
    bool aliving;

    [Header("States")]
    [SerializeField] private StatueSpawnState spawnState;
    [SerializeField] private StatueStayState stayState;
    [SerializeField] private StatueWalkState walkState;
    [SerializeField] private StatueAgressiveState agressiveState;
    public StatueState currentState { get; private set; }

    [HideInInspector] public bool attack;

    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        if (legsAnimator == null)
            legsAnimator = legs.GetComponent<Animator>();

        if (spawnState == null)
            spawnState = statesGameObject.GetComponent<StatueSpawnState>();
        if (stayState == null)
            stayState = statesGameObject.GetComponent<StatueStayState>();
        if (walkState == null)
            walkState = statesGameObject.GetComponent<StatueWalkState>();
        if (agressiveState == null)
            agressiveState = statesGameObject.GetComponent<StatueAgressiveState>();
    }

    private void Awake()
    {
        health = maxHealth;

        target = null;
        agressive = false;
        aliving = false;

        stay = stayOnAwake;
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

        if (!alreadySpawnedOnStart)
            SetState(spawnState);
        else if (stayOnAwake)
            SetState(stayState);
        else
            SetState(walkState);
    }

    private void Update()
    {
        if (currentState.isFinished)
        {
            if (agressive) SetState(agressiveState);
            else SetState(walkState);
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

    public void SetMovementDirection(Vector2 direction)
    {
        movementDirection = direction.normalized;
    }

    public override void Attack()
    {
        if (nextAttackTime > 0) return;

        bodyAnimator.SetTrigger("Attack");

        AudioManager.instance.PlaySoundEffect(shootSE, rb.position, (1 / firingRate) * 1.5f);

        Vector3 bulletAngle = attackPoint.eulerAngles;
        bulletAngle.z += UnityEngine.Random.Range(-spread, spread);

        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.Euler(bulletAngle));
        bullet.GetComponent<Bullet>().SetBulletParameters(damage);

        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        brb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);

        nextAttackTime = 1 / firingRate;
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

        SetState(walkState);

        effect.GetComponent<Animator>().SetFloat("Speed", 2 / alivingTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, alivingTime);

        aliving = false;
    }

    protected override void PlayerDeath()
    {
        if (currentState == stayState) return;

        agressive = false;
        SetState(walkState);
    }

    private void CountTimeVariables()
    {
        if (nextAttackTime > 0) nextAttackTime -= Time.deltaTime;
    }

    private void SetState(StatueState state)
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
            currentState.statue = this;
            currentState.Init();
        }
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        if (currentState == spawning)
            return;
         
        if (currentState != stayState)
        {
            health -= damage;

            var soundEffect = Instantiate(damageSoundPrefab);
            soundEffect.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            Destroy(soundEffect, 2f);

            if (currentState != agressiveState)
                SetState(agressiveState);

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
        else
        {
            GameObject effect = Instantiate(bulletsAftereffects, new Vector3(attack.position.x, attack.position.y, 1), attack.rotation);
            Destroy(effect, 0.3f);
        }
    }

    private void Move()
    {
        if (rb.bodyType == RigidbodyType2D.Static) return;

        rb.velocity = movementDirection * speed;
    }

    private void Animate()
    {
        bodyAnimator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void RotateBody()
    {
        if ((movementDirection == Vector2.zero && !attack) || (currentState == stayState))
            return;

        if (targetOnAim)
        {
            Vector2 dir = ((Vector2)target.transform.position - rb.position).normalized;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(legs.transform.eulerAngles.z, targetAngle - 90, ref turnSmoothVelocity, aimingTurnSmoothTime);

            //Vector3 legsRotation = legs.transform.eulerAngles;
            rb.transform.rotation = Quaternion.Euler(0, 0, angle);
            // legs.transform.eulerAngles = legsRotation;
            legs.transform.rotation = Quaternion.Euler(0, 0, angle);
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

        SetState(walkState);

        EffectsManager.instance.PlaySoundEffect(EffectsStorage.instance.enemySpawnSound, 3f, 0.9f, 1.1f);

        effect.GetComponent<Animator>().SetFloat("Speed", 2 / spawningTime);
        effect.GetComponent<Animator>().Play("Disappear");
        Destroy(effect, spawningTime / 2);

        spawning = false;
    }
}
