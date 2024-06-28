using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacineAgressiveState : RacineState
{
    [Header("Behaviour")]
    public float attackDistance;
    public float distanceToMissPlayer;

    public override void Init()
    {
        isFinished = false;

        racine.target = Player.instance.transform;

        racine.targetOnAim = true; 

    }

    public override void Run()
    {
        if (racine.attack)
            return;

        racine.ExecutePath(); 

        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, racine.rb.position);

        if (CheckPlayer() && distanceToPlayer < attackDistance)
            racine.Attack(); 

        if (distanceToPlayer > distanceToMissPlayer)
        {
            racine.agressive = false;
            isFinished = true;
        }
    }

    public override void Exit()
    {
        racine.movementDirection = Vector2.zero;
    }

    private bool CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(racine.rb.position, Player.instance.rb.position - racine.rb.position, attackDistance);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall"))
                return false;
            else if (hit.transform.gameObject.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToMissPlayer);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackDistance); 
    }
}
