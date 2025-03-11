using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaceAttackState : BronzeHeraclesState
{
    [Header("Behavior")]
    public float attackDistance;
    [HideInInspector] public bool maceTaken;
    public int maceDamage;
    public float timeTakeMace;
    public float timeAttackMace;
    public float timeRemoveMace;
    [Header("Sound")]
    [SerializeField] protected SoundEffect hitSE;
    public override void Init()
    {
        isFinished = false;

        bronzeHeracles.target = Player.instance.transform;

        bronzeHeracles.targetOnAim = true;


        bronzeHeracles.attack = true;

        maceTaken = false;
    }
    public override void Run()
    {
        bronzeHeracles.ExecutePath();
        if (!maceTaken && !bronzeHeracles.isTakeMace) {
            bronzeHeracles.TakeMace();
        }
        if (maceTaken && bronzeHeracles.attack && !bronzeHeracles.isAttackMace) {
            float distanceToPlayer = Vector2.Distance(Player.instance.rb.position, bronzeHeracles.rb.position);
            if (CheckPlayer() && distanceToPlayer < attackDistance)
                bronzeHeracles.MaceAttack();
        }
        if (!bronzeHeracles.attack && !bronzeHeracles.isRemoveMace) {
            bronzeHeracles.RemoveMace();
            isFinished = true;
        }
    }
    private bool CheckPlayer()
    {
        var hits = Physics2D.RaycastAll(bronzeHeracles.rb.position, Player.instance.rb.position - bronzeHeracles.rb.position, attackDistance);

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Wall"))
                return false;
            else if (hit.transform.gameObject.CompareTag("Player"))
                return true;
        }

        return false;
    }
}
