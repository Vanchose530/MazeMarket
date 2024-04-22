using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAgressiveState : ZombieState
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

        zombie.currentSpeed = zombie.agressiveSpeed;

        zombie.target = Player.instance.transform;

        zombie.targetOnAim = true;
    }

    public override void Run()
    {
        zombie.ExecutePath();

        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, zombie.rb.position);

        if (!CheckObstacle() && distanceToPlayer < attackDistance) zombie.Attack();

        if (attackObstacles && zombie.rb.velocity.magnitude < zombie.speed && CheckObstacle()) zombie.Attack();

        if (distanceToPlayer > distanceToMissPlayer)
        {
            zombie.agressive = false;
            isFinished = true;
        }
    }

    public override void Exit()
    {
        zombie.movementDirection = Vector2.zero;
        zombie.target = null;
    }

    private bool CheckObstacle()
    {
        RaycastHit2D hit;

        if (attackObstacles)
            hit = Physics2D.Raycast(zombie.rb.position, zombie.movementDirection, checkObstaclesDistance, obstacleLayer);
        else
            hit = Physics2D.Raycast(zombie.rb.position, ((Vector2)zombie.target.transform.position - zombie.rb.position).normalized, checkObstaclesDistance, obstacleLayer);

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
