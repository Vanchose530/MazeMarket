using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GoplitAgressiveState : GoplitState
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float timeAttack;
    [SerializeField] private GameObject spear;

    public override void Init() {
        goplit.attack = true;
    }

    public override void Run() {
        StartCoroutine("StartRun");
        
    }
    public IEnumerator StartRun() {
        
        //bodyAnimator.SetTrigger("Attack");
        goplit.movementDirection = Vector2.zero;
        
        yield return new WaitForSeconds(5f);

        if (goplit.attack)
        {
            goplit.transform.position = Vector3.MoveTowards(goplit.transform.position, goplit.targetEnd.position, speed * Time.deltaTime);
            timeAttack -= Time.deltaTime;
            if (timeAttack <= 0)
            {
                goplit.attack = false;
            }
            spear.GetComponent<Collider2D>().enabled = true;
        }
        else {
            spear.GetComponent<Collider2D>().enabled = false;
        }
        

        goplit.targetOnAim = false;

        yield return new WaitForSeconds(3f);
        spear.GetComponent<Collider2D>().enabled = false;
        isFinished = true;
        goplit.attack = false;
        goplit.agressive = false;
        goplit.recover = true;
        //bodyAnimator.SetTrigger("Default");
    }



}
