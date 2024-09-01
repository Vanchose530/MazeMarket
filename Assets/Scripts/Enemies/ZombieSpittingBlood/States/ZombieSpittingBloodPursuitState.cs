using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpittingBloodPursuitState : ZombieSpittingBloodState
{
    [Header("Behaviour")]
    public float attackDistance;
    public float distanceToMissPlayer;
    public bool attackObstacles;
    public float checkObstaclesDistance;
    public LayerMask obstacleLayer;

    public override void Init()
    {
        isFinished = false;

        zombieSpittingBlood.currentSpeed = zombieSpittingBlood.agressiveSpeed;

        zombieSpittingBlood.target = Player.instance.transform;

        zombieSpittingBlood.targetOnAim = true;
    }

    public override void Run()
    {
        zombieSpittingBlood.ExecutePath();

        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, zombieSpittingBlood.rb.position);

        if (!CheckObstacle() && distanceToPlayer < attackDistance) 
        {
            zombieSpittingBlood.Attack();
            
        }

        if (attackObstacles && zombieSpittingBlood.rb.velocity.magnitude < zombieSpittingBlood.speed && CheckObstacle()) 
        {
            zombieSpittingBlood.Attack();
        } 

        if (distanceToPlayer > distanceToMissPlayer)
        {
            zombieSpittingBlood.agressive = false;
            isFinished = true;
        }
    }

    public override void Exit()
    {
        zombieSpittingBlood.movementDirection = Vector2.zero;
        zombieSpittingBlood.target = null;
    }

    private bool CheckObstacle()
    {
        RaycastHit2D hit;

        if (attackObstacles)
            hit = Physics2D.Raycast(zombieSpittingBlood.rb.position, zombieSpittingBlood.movementDirection, checkObstaclesDistance, obstacleLayer);
        else
            hit = Physics2D.Raycast(zombieSpittingBlood.rb.position, ((Vector2)zombieSpittingBlood.target.transform.position - zombieSpittingBlood.rb.position).normalized, checkObstaclesDistance, obstacleLayer);

        return hit;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToMissPlayer);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackDistance);

        if (attackObstacles)
        {
            Gizmos.color = Color.grey;

            Gizmos.DrawWireSphere(transform.position, checkObstaclesDistance);
        }
    }
}

