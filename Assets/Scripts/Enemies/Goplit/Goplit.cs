using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Goplit : Enemy, IDamagable
{
    [Header("Target")]
    public Transform targetEnd;
    [Header ("Animators")]
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    //[SerializeField] private Animator legsAnimator;

    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;

    [Header("Goplit States")]
    [SerializeField] private GoplitSpawnState spawnState;
    [SerializeField] private GoplitPassiveState passiveState;
    [SerializeField] private GoplitPursuitState pursuitState;
    [SerializeField] public GoplitRecoveryState recoveryState;
    [SerializeField] private GoplitAgressiveState agressiveState;
    public GoplitState currentState { get; private set; }

    public bool attack { get; set; }
    public bool recover { get; set; }
    private void OnValidate()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (seeker == null)
            seeker = GetComponent<Seeker>();
        //if (legsAnimator == null)
        //    legsAnimator = legs.GetComponent<Animator>();

        if (spawnState == null)
            spawnState = statesGameObject.GetComponent<GoplitSpawnState>();
        if (passiveState == null)
            passiveState = statesGameObject.GetComponent<GoplitPassiveState>();
        if (recoveryState == null)
            recoveryState = statesGameObject.GetComponent<GoplitRecoveryState>();
        if (pursuitState == null)
            pursuitState = statesGameObject.GetComponent<GoplitPursuitState>();
        if (agressiveState == null)
            agressiveState = statesGameObject.GetComponent<GoplitAgressiveState>();
    }
    private void Awake()
    {
        health = maxHealth;

        target = null;
        agressive = false;
        attack = false;

        recover = false;
    }
    private void Start()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        SetState(passiveState);
        
    }

    private void Update()
    {

        if (currentState.isFinished)
        {
            if (agressive)
                SetState(pursuitState);
            else if (recover)
                SetState(recoveryState);
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
        //legsAnimator.SetFloat("Speed", rb.velocity.magnitude);
    }


    public override void Attack()
    {
        SetState(agressiveState);
        currentState.Run();
        
    }


    public override void Spawn()
    {
        StartCoroutine("StartSpawning");
    }

    public void TakeDamage(int damage, Transform attack = null)
    {
        if (spawning)
            return;

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
        if (attack)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = movementDirection * speed;
    }
}
