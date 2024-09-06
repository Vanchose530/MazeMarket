using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueStayState : StatueState
{
    [Header("Behaviour")]
    public float distanceToFindPlayer;

    public override void Init()
    {
        isFinished = false;

        statue.LockRigidbody(true);
    }

    public override void Run()
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
            return;

        if (Vector2.Distance(Player.instance.rb.position, statue.rb.position) < distanceToFindPlayer)
            CheckPlayer();
    }

    public override void Exit()
    {
        statue.LockRigidbody(false);
    }

    private void CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(statue.rb.position, Player.instance.rb.position - statue.rb.position, distanceToFindPlayer);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall")) return;
            else if (hit.transform.gameObject.CompareTag("Player"))
            {
                statue.Alive();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceToFindPlayer);
    }
}
