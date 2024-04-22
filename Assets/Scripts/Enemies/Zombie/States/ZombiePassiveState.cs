using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePassiveState : ZombieState
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

        zombie.currentSpeed = zombie.speed;

        stay = true;
        ResetTimeToWalkInOneTurn();
    }

    public override void Run()
    {
        CountTime();

        if (walkInOneTurnTime <= 0 || (!stay && zombie.rb.velocity.sqrMagnitude < zombie.speed * zombie.speed)) SetRandomMovement();

        if (Vector2.Distance(Player.instance.rb.position, zombie.rb.position) < distanceToFindPlayer) CheckPlayer();
    }

    private void CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(zombie.rb.position, Player.instance.rb.position - zombie.rb.position, distanceToFindPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall")) return;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                zombie.agressive = true;
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
            zombie.movementDirection = Vector2.zero;
        }
        else
        {
            stay = false;
            Vector2 direction = Random.insideUnitCircle.normalized;
            zombie.movementDirection = direction;
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
