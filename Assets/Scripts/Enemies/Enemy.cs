using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth;
    public int health { get; protected set; }

    [Header("Movement")]
    public float speed;

    private Vector2 _movementDirection = Vector2.zero;
    public Vector2 movementDirection
    {
        get { return _movementDirection; }
        set { _movementDirection = value.normalized; }
    }

    private Transform _target;
    public Transform target
    {
        get { return _target; }
        set { targetOnAim = false; _target = value; }
    }

    [SerializeField] protected float nextWaypointDistance;
    [HideInInspector] public bool targetOnAim;
    protected bool walk; // TODO - нормализовать анимации отслеживая показатель walk в аниматоре

    public Path path { get; private set; }
    private int _currentWaypoint = 0;
    public int currentWaypoint
    {
        get { return _currentWaypoint; }
        private set { _currentWaypoint = value; }
    }
    bool reachedEndOfPath = false;

    [Header("Attack")]
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected int damage;

    [Header("States")]
    [SerializeField] protected GameObject statesGameObject;
    [HideInInspector] public bool agressive;

    [Header("Spawn")]
    public bool alreadySpawnedOnStart;
    [SerializeField] protected float spawningTime;
    protected bool spawning;

    [Header("Components (setted on validate)")]
    [SerializeField] protected Seeker seeker;
    [SerializeField] protected Rigidbody2D _rb;
    public Rigidbody2D rb
    {
        get { return _rb; }
        protected set { _rb = value; } 
    }

    public event Action onEnemyDeath;

    protected void EnemyDeathEvent()
    {
        if (onEnemyDeath != null)
            onEnemyDeath();
        SimpleScoreCounter.instance.AddDefeatedEnemy(); // для счёта кол-ва поверженных врагов, подобную логику стоит размещать в событиях
    }

    public virtual void ExecutePath(bool aStar = true)
    {
        if (path == null) return;

        if (aStar)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

            movementDirection = direction;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            movementDirection = ((Vector2)target.transform.position - rb.position).normalized;
        }
    }

    public abstract void Attack();

    protected void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathCompleted);
    }

    protected void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected abstract void PlayerDeath();

    public abstract void Spawn();
}
