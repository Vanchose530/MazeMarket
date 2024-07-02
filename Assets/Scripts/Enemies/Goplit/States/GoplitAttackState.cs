using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GoplitAttackState : GoplitState
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private GameObject spear;
    float time;
    public override void Init()
    {
        isFinished = false;

        goplit.agressive = true;
        goplit.attack = true;
        goplit.recover = false;


    }

    public override void Run()
    {
        goplit.Rush();
    }

    //public IEnumerator StartRun()
    //{
    //    goplit.movementDirection = Vector2.zero;
    //    float time = goplit.timeAttack;

    //    yield return new WaitForSeconds(goplit.aimingTime);

    //    if (goplit.attack)
    //    {
    //        // для перемещения использовать goplit.moveDirection = *направление*
    //        goplit.transform.position = Vector3.MoveTowards(goplit.transform.position, goplit.targetEnd.position, speed * Time.deltaTime);
    //        time -= Time.deltaTime;
    //        if (time <= 0)
    //        {
    //            goplit.attack = false;
    //        }
    //        spear.GetComponent<Collider2D>().enabled = true;
    //    }
    //    else
    //    {
    //        spear.GetComponent<Collider2D>().enabled = false;
    //    }
        
    //    goplit.targetOnAim = false;

    //    yield return new WaitForSeconds(goplit.timeAttack);

    //    //goplit.bodyAnimator.SetFloat("AttackEnd Multiplier", 1 / goplit.attackEnd);
    //    //goplit.bodyAnimator.SetTrigger("AttackEnd");

    //    yield return new WaitForSeconds(goplit.attackEnd);

    //    spear.GetComponent<Collider2D>().enabled = false;

    //    isFinished = true;

    //    goplit.attack = false;
    //    goplit.agressive = false;
    //    goplit.recover = true;
    //}
}
