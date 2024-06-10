using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Goplit : Enemy, IDamagable
{
    [Header ("Attack")]


    [Header("Effects")]
    [SerializeField] private GameObject damageEffect;

    [Header("Goplit States")]
    [SerializeField] private GoplitSpawnState spawnState;
    [SerializeField] private GoplitPassiveState passiveState;
    [SerializeField] private GoplitAgressiveState agressiveState;
    [SerializeField] private GoplitRecoveryState recoveryState;
    public GoplitState currentState { get; private set; }

    public bool attack { get; private set; }
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
        if (agressiveState == null)
            agressiveState = statesGameObject.GetComponent<GoplitAgressiveState>();
    }
    private void Awake()
    {
        health = maxHealth;

        target = null;
        agressive = false;
        attack = false;

        //SetAnimationSettings();
    }
    private void Start()
    {
        SetState(passiveState);
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
    }
    private void SetState(GoplitState state)
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

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }


    public override void Spawn()
    {
        throw new System.NotImplementedException();
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
    private void Move()
    {
        if (attack)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = movementDirection * speed;
    }
}
