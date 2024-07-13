using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoplitStayState : GoplitState
{
    [Header("Behaviour")]
    public float distanceToFindPlayer;

    public override void Init()
    {
        isFinished = false;

        goplit.LockRigidbody(true);
    }

    public override void Run()
    {
        if (Vector2.Distance(Player.instance.rb.position, goplit.rb.position) < distanceToFindPlayer) CheckPlayer();
    }

    public override void Exit()
    {
        goplit.LockRigidbody(false);
    }

    private void CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(goplit.rb.position, Player.instance.rb.position - goplit.rb.position, distanceToFindPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall")) return;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                goplit.Alive();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceToFindPlayer);
    }
}
