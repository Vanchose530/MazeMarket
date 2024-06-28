using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GoplitAgressiveState : GoplitState
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private GameObject spear;

    public override void Init() {
        goplit.agressive = true;
        goplit.attack = true;
        isFinished = false;
        goplit.recover = false;
    }

    public override void Run() {
        StartCoroutine("StartRun");
    }
    public IEnumerator StartRun() {
        
        
        goplit.movementDirection = Vector2.zero;
        float time = goplit.timeAttack;
        yield return new WaitForSeconds(goplit.aimingTime);

        if (goplit.attack)
        {
            goplit.transform.position = Vector3.MoveTowards(goplit.transform.position, goplit.targetEnd.position, speed * Time.deltaTime);
            time -= Time.deltaTime;
            if (time <= 0)
            {
                goplit.attack = false;
            }
            spear.GetComponent<Collider2D>().enabled = true;
        }
        else {
            spear.GetComponent<Collider2D>().enabled = false;
        }
        

        goplit.targetOnAim = false;

        
        yield return new WaitForSeconds(goplit.timeAttack + goplit.attackEnd);

        spear.GetComponent<Collider2D>().enabled = false;
        isFinished = true;
        goplit.attack = false;
        goplit.agressive = false;
        goplit.recover = true;
        
    }



}
