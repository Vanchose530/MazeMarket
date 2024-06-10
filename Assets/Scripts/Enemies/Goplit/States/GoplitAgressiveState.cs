using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoplitAgressiveState : GoplitState
{
    [Header("Behaviour")]
    public float attackDistance;
    public float distanceToMissPlayer;

    public override void Init()
    {
        isFinished = false;

        goplit.target = Player.instance.transform;

        goplit.targetOnAim = true;
    }

    public override void Run()
    {
        if (goplit.attack)
            return;
        goplit.ExecutePath();

        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, goplit.rb.position);

        if (CheckPlayer() && distanceToPlayer < attackDistance)
            goplit.Attack();

        if (distanceToPlayer > distanceToMissPlayer)
        {
            goplit.agressive = false;
            isFinished = true;
        }
    }

    public override void Exit()
    {
        goplit.movementDirection = Vector2.zero;
    }

    private bool CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(goplit.rb.position, Player.instance.rb.position - goplit.rb.position, attackDistance);

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

