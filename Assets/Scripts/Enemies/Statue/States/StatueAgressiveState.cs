using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAgressiveState : StatueState
{
    [Header("Behaviour")]
    public float timeToStartShooting;
    float timeToStartShootingBuffer;
    public float distanceToMissPlayer;
    public float distanceToAttackPlayer;

    public override void Init()
    {
        isFinished = false;

        statue.target = Player.instance.transform;

        timeToStartShootingBuffer = timeToStartShooting;
    }

    public override void Run()
    {
        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, statue.rb.position);

        if (CheckPlayer() && distanceToPlayer < distanceToAttackPlayer)
        {
            statue.attack = true;
            statue.targetOnAim = true;

            CountTimeToStartShooting();
            statue.SetMovementDirection(Vector2.zero);

            if (timeToStartShootingBuffer <= 0) 
                statue.Attack();
        }
        else if (distanceToPlayer > distanceToMissPlayer)
        {
            statue.agressive = false;
            isFinished = true;
        }
        else
        {
            statue.attack = false;
            statue.targetOnAim = false;
            timeToStartShootingBuffer = timeToStartShooting;
            statue.ExecutePath();
        }
    }

    public override void Exit()
    {
        statue.attack = false;
        statue.target = null;
    }

    private void CountTimeToStartShooting()
    {
        if (timeToStartShootingBuffer > 0) timeToStartShootingBuffer -= Time.deltaTime;
    }

    private bool CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(statue.rb.position, Player.instance.rb.position - statue.rb.position, distanceToMissPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall") || hit.transform.gameObject.CompareTag("Hight Deoration")) return false;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToMissPlayer);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, distanceToAttackPlayer);
    }
}
