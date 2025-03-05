using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool active { get; private set; }

    [Header("Setup")]
    [SerializeField] private Animator animator;

    private void OnValidate()
    {
        animator = animator == null ? GetComponent<Animator>() : animator;
    }

    private void Start()
    {
        active = false;
        animator.Play("Hide");
    }

    public void Activate()
    {
        animator.Play("Appear");
        active = true;
    }
}
