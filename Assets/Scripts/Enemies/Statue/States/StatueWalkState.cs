using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueWalkState : StatueState
{
    [Header("Behaviour")]
    public float minWalkInOneTurnTime;
    public float maxWalkInOneTurnTime;
    float walkInOneTurnTime;
    public int stayChancePercents;
    bool stay;
    public float distanceToFindPlayer;

    public override void Init()
    {
        isFinished = false;

        stay = false;
        ResetTimeToWalkInOneTurn();
    }

    public override void Run()
    {
        CountTime();

        if (walkInOneTurnTime <= 0 || (!stay && statue.rb.velocity.sqrMagnitude < statue.speed * statue.speed)) SetRandomMovement();

        if (Vector2.Distance(Player.instance.rb.position, statue.rb.position) < distanceToFindPlayer)
        {
            statue.agressive = true;
            isFinished = true;
        }
    }

    private void CountTime()
    {
        if (walkInOneTurnTime > 0) { walkInOneTurnTime -= Time.deltaTime; }
    }

    private void SetRandomMovement()
    {
        ResetTimeToWalkInOneTurn();

        int i = Random.Range(0, 100);

        if (i < stayChancePercents)
        {
            stay = true;
            statue.SetMovementDirection(Vector2.zero);
        }
        else
        {
            stay = false;
            Vector2 direction = Random.insideUnitCircle.normalized;
            statue.SetMovementDirection(direction);
        }
    }

    private void ResetTimeToWalkInOneTurn()
    {
        walkInOneTurnTime = Random.Range(minWalkInOneTurnTime, maxWalkInOneTurnTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToFindPlayer);
    }
}
