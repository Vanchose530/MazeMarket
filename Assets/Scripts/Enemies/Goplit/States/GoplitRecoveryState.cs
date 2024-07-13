using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoplitRecoveryState : GoplitState
{
    [Header("Behaviour")]
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float minRecoveryTime;
    [SerializeField] private float maxRecoveryTime;
    [SerializeField] private float minWalkInOneTurnTime;
    [SerializeField] private float maxWalkInOneTurnTime;
    float walkInOneTurnTime;
    bool rightTurn;

    private float recoveryTime;

    public override void Init()
    {
        isFinished = false;

        recoveryTime = Random.Range(minRecoveryTime, maxRecoveryTime);

        goplit.agressive = false;

        ResetTimeToWalkInOneTurn();
    }

    public override void Run()
    {
        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, goplit.rb.position);
        Debug.Log(distanceFromPlayer);
        Vector2 vectorFromPlayer = (goplit.rb.position - Player.instance.rb.position).normalized;
        Vector2 moveVector = Vector2.zero;

        if (walkInOneTurnTime <= 0 || goplit.rb.velocity.magnitude < goplit.speed / 3)
        {
            ChangeTurn();
            ResetTimeToWalkInOneTurn();
        }

        if (distanceToPlayer < distanceFromPlayer)
        {
            moveVector += vectorFromPlayer;
        }

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

        goplit.movementDirection = moveVector;

        CountTimeVariables();

        
    }

    private void CountTimeVariables()
    {
        if (recoveryTime > 0)
            recoveryTime -= Time.deltaTime;
        else
        {
            isFinished = true;
        }
        if (walkInOneTurnTime > 0)
            walkInOneTurnTime -= Time.deltaTime;
    }

    private void ResetTimeToWalkInOneTurn()
    {
        walkInOneTurnTime = Random.Range(minWalkInOneTurnTime, maxWalkInOneTurnTime);
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
        Gizmos.color = Color.grey;

        Gizmos.DrawWireSphere(transform.position, distanceFromPlayer);
    }
}
