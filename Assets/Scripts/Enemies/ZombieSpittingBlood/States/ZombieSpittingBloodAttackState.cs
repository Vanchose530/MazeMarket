using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpittingBloodAttackState : ZombieSpittingBloodState
{
    [Header("Behaviour")]
    [SerializeField] private float distanceRun;
    [SerializeField] private float distanceAttack;
    [SerializeField] private float distancePursuit;
    [SerializeField] private int minCountShoot;
    [SerializeField] private int maxCountShoot;
    [SerializeField] private float minWalkInOneTurnTime;
    [SerializeField] private float maxWalkInOneTurnTime;
    float walkInOneTurnTime;
    bool rightTurn;

    public int attackTime;

    public override void Init()
    {
        isFinished = false;

        attackTime = Random.Range(minCountShoot, maxCountShoot);

        ResetTimeToWalkInOneTurn();
    }

    public override void Run()
    {

        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position,zombieSpittingBlood.rb.position);
        Vector2 vectorFromPlayer = (zombieSpittingBlood.rb.position - Player.instance.rb.position).normalized;
        Vector2 moveVector = Vector2.zero;

        

        if (distanceToPlayer <= distanceRun)
        {
            if (walkInOneTurnTime <= 0 || zombieSpittingBlood.rb.velocity.magnitude < zombieSpittingBlood.speed / 3)
            {
                ChangeTurn();
                ResetTimeToWalkInOneTurn();
            }
            moveVector += vectorFromPlayer;

            zombieSpittingBlood.movementDirection = moveVector;

            if (rightTurn)
            {
                moveVector.x = vectorFromPlayer.y;
                moveVector.y = -vectorFromPlayer.x;
            }
            else
            {
                moveVector.x = vectorFromPlayer.y;
                moveVector.y = vectorFromPlayer.x;
            }

            
        }
        else if (distanceToPlayer >= distanceRun && distanceToPlayer <= distanceAttack) 
        {

            zombieSpittingBlood.Attack();

        }
        else if (distanceToPlayer >= distanceAttack) 
        {

            zombieSpittingBlood.ExecutePath();

            zombieSpittingBlood.target = Player.instance.transform;

            zombieSpittingBlood.targetOnAim = true;

        }


        EndAttack();
    }


    private void ResetTimeToWalkInOneTurn()
    {
        walkInOneTurnTime = Random.Range(minWalkInOneTurnTime, maxWalkInOneTurnTime);
    }

    private void EndAttack() {

        if (attackTime == 0) {
            zombieSpittingBlood.agressive = false;
            zombieSpittingBlood.attack = false;
            zombieSpittingBlood.SetState(zombieSpittingBlood.recoveryState);
            isFinished = true;
        }

    }

    private void ChangeTurn()
    {
        if (rightTurn)
            rightTurn = false;
        else
            rightTurn = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, distanceRun);

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, distanceAttack);

        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.position, distancePursuit);
    }

}
