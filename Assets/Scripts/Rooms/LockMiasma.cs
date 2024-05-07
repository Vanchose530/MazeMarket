using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMiasma : MonoBehaviour
{
    public bool lockStatus { get; private set; } = true;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        boxCollider.enabled = false;
        lockStatus = false;
    }

    public void Lock(float time = 1f)
    {
        if (lockStatus)
            return;

        //animator.SetFloat("Lock/Unlock Multiplier", 1 / time);
        //animator.Play("Lock");
        StartCoroutine(StartLock(time));
        lockStatus = true;
    }

    public void Unlock(float time = 1f, bool destroy = false)
    {
        if (destroy)
            Destroy(this.gameObject, time * 1.1f);

        if (!lockStatus)
            return;

        //animator.SetFloat("Lock/Unlock Multiplier", 1 / time);
        //animator.Play("Unlock");
        StartCoroutine(StartUnlock(time));
        lockStatus = false;
    }

    private IEnumerator StartLock(float time)
    {
        boxCollider.enabled = true;

        while (spriteRenderer.color.a < 1)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.01f);
            Debug.Log(spriteRenderer.color.a);
            yield return new WaitForSecondsRealtime(0.01f / time);
        }
    }

    private IEnumerator StartUnlock(float time)
    {
        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.01f);
            yield return new WaitForSecondsRealtime(0.01f / time);
        }

        boxCollider.enabled = false;
    }
}
