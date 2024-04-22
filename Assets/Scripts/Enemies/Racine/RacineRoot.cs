using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacineRoot : MonoBehaviour
{
    [SerializeField] private string ignoreTag;

    [Header("Components")]
    [SerializeField] private Animator animator;

    public int damage { private get; set; }

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void RootForward(float speed)
    {
        animator.SetFloat("Root Speed", speed);
        animator.Play("Root Forward");
    }

    public void RootBack(float speed)
    {
        animator.SetFloat("Root Speed", speed);
        animator.Play("Root Back");
    }

    public void DisableRoot()
    {
        animator.Play("Disable");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ignoreTag))
            return;

        IDamagable obj = collision.GetComponent<IDamagable>();

        if (obj != null)
            obj.TakeDamage(damage, transform);
    }
}
