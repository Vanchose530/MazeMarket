using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoplitPassiveState : GoplitState
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

        goplit.target = null;

        stay = true;
        ResetTimeToWalkInOneTurn();
    }

    public override void Run()
    {
        CountTime();
        if (walkInOneTurnTime <= 0 || (!stay && goplit.rb.velocity.sqrMagnitude < goplit.speed * goplit.speed)) SetRandomMovement();
        if (Vector2.Distance(Player.instance.rb.position, goplit.rb.position) < distanceToFindPlayer) CheckPlayer();
    }

    private void CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(goplit.rb.position, Player.instance.rb.position - goplit.rb.position, distanceToFindPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall")) return;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                goplit.agressive = true;
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
            goplit.movementDirection = Vector2.zero;
        }
        else
        {
            stay = false;
            Vector2 direction = Random.insideUnitCircle.normalized;
            goplit.movementDirection = direction;
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
