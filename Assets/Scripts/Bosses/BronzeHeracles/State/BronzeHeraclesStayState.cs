using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeHeraclesStayState : BronzeHeraclesState
{
    [Header("Behaviour")]
    public float distanceToFindPlayer;

    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.LockRigidbody(true);
    }

    public override void Run()
    {
        if (Vector2.Distance(Player.instance.rb.position, bronzeHeracles.rb.position) < distanceToFindPlayer) CheckPlayer();
    }

    public override void Exit()
    {
        bronzeHeracles.LockRigidbody(false);
    }

    private void CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(bronzeHeracles.rb.position, Player.instance.rb.position - bronzeHeracles.rb.position, distanceToFindPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall")) return;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                bronzeHeracles.Alive();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceToFindPlayer);
    }
}
