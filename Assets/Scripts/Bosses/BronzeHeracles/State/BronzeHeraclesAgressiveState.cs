using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeHeraclesAgressiveState : BronzeHeraclesState
{
    [Header("Behaviour")]
    public bool attackObstacles;
    public float checkObstaclesDistance;
    public float distanceToArcherState;
    public float distanceToStoneThrowState;
    public float distanceMaceAttackState;
    public LayerMask obstacleLayer;

    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.target = Player.instance.transform;

        bronzeHeracles.targetOnAim = true;
    }

    public override void Run()
    {
        bronzeHeracles.ExecutePath();

        float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, bronzeHeracles.rb.position);

        if (distanceToPlayer < distanceToArcherState) {
            Debug.Log("Archer");
        }
        if (distanceToPlayer < distanceToStoneThrowState)
        {
            Debug.Log("Stone");
        }
        if (distanceToPlayer < distanceMaceAttackState)
        {
            Debug.Log("Mace");
        }
    }

    public override void Exit()
    {
        bronzeHeracles.movementDirection = Vector2.zero;
        bronzeHeracles.target = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToArcherState);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, distanceToStoneThrowState);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, distanceMaceAttackState);


        
    }
}
