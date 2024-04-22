using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacinePassiveState : RacineState
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

        racine.target = null;

        stay = true;
        ResetTimeToWalkInOneTurn();
    }

    public override void Run()
    {
        CountTime();

        if (walkInOneTurnTime <= 0 || (!stay && racine.rb.velocity.sqrMagnitude < racine.speed * racine.speed)) SetRandomMovement();

        if (Vector2.Distance(Player.instance.rb.position, racine.rb.position) < distanceToFindPlayer) CheckPlayer();
    }

    private void CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(racine.rb.position, Player.instance.rb.position - racine.rb.position, distanceToFindPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall")) return;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                racine.agressive = true;
                isFinished = true;
            }
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
            racine.movementDirection = Vector2.zero;
        }
        else
        {
            stay = false;
            Vector2 direction = Random.insideUnitCircle.normalized;
            racine.movementDirection = direction;
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
