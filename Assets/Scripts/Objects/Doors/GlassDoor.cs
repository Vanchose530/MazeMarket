using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlassDoor : Door
{
    const string OUTLINE_KEYWORD = "OUTLINE_ON";

    [Header("Door Locked")]
    [SerializeField] private Renderer[] doorRenderers;
    private GameObject lockEffect;

    [Header("Glass Door")]
    private bool _canOpen;

    [Header("Effects")]
    [SerializeField] private GameObject closeAtAllEffect;

    [Header("Sound Effects")]
    [SerializeField] private GameObject closeAtAllSound;
    [SerializeField] private GameObject nowOpenSound;
    [HideInInspector] public bool canOpen
    {
        get { return _canOpen; }
        set
        {
            SetOutline(value);

            if (value && lockEffect != null)
            {
                lockEffect.GetComponent<Animator>().Play("Disappear");
                EffectsManager.instance.PlaySoundEffect(nowOpenSound, transform.position, 5f, 0.9f, 1.1f);
                Destroy(lockEffect, 5f);
            }
            else if (!value && lockEffect == null)
            {
                lockEffect = Instantiate(closeAtAllEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f), transform.rotation);
                EffectsManager.instance.PlaySoundEffect(closeAtAllSound, transform.position, 5f, 0.9f, 1.1f);
            }

            _canOpen = value;
        }
    }

    private void OnValidate()
    {
        doorRenderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        canOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canOpen)
        {
            Open(openCloseSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Close(openCloseSpeed);
        }
    }

    public void Close(float speed, bool closeAtAll)
    {
        animator.SetFloat("Speed", -speed);
        animator.SetTrigger("Interact");
        canOpen = !closeAtAll; 
    }

    void SetOutline(bool outline)
    {
        foreach (var rend in doorRenderers)
        {
            if (outline)
                rend.material.DisableKeyword(OUTLINE_KEYWORD);
            else
                rend.material.EnableKeyword(OUTLINE_KEYWORD); 
        }
    }
}
