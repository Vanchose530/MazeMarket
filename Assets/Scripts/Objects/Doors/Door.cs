using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Door : MonoBehaviour
{
    [Header("Door")]
    public float openCloseSpeed;
    protected Animator animator;

    public void Open(float speed)
    {
        animator.SetFloat("Speed", speed);
        animator.SetTrigger("Interact");
    }

    public void Close(float speed)
    {
        animator.SetFloat("Speed", -speed);
        animator.SetTrigger("Interact");
    }
}
