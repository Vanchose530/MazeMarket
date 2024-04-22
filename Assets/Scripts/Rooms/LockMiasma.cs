using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMiasma : MonoBehaviour
{
    public bool lockStatus { get; private set; } = true;

    [Header("Components")]
    [SerializeField] private Animator animator;

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Lock(float time = 1f)
    {
        if (lockStatus)
            return;

        animator.SetFloat("Lock/Unlock Multiplier", 1 / time);
        animator.Play("Lock");
        lockStatus = true;
    }

    public void Unlock(float time = 1f, bool destroy = false)
    {
        if (destroy)
            Destroy(this.gameObject, time * 1.1f);

        if (!lockStatus)
            return;

        animator.SetFloat("Lock/Unlock Multiplier", 1 / time);
        animator.Play("Unlock");
        lockStatus = false;
    }
}
