using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntObjsTester : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer sp;

    private Color baseColor;
    [SerializeField] private Color canInteractColor;
    [SerializeField] private Color interactColor;

    bool canInteract = false;

    private void OnValidate()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        baseColor = new Color(sp.color.r, sp.color.g, sp.color.b);
    }

    public void Interact(Player player)
    {
        StartCoroutine(InteractAction());
    }

    public void CanInteract(Player player)
    {
        sp.color = canInteractColor;
        canInteract = true;
    }

    public void CanNotInteract(Player player)
    {
        sp.color = baseColor;
        canInteract = false;
    }

    private IEnumerator InteractAction()
    {
        sp.color = interactColor;
        yield return new WaitForSecondsRealtime(1f);
        if (canInteract)
            sp.color = canInteractColor;
        else
            sp.color = baseColor;
    }
}
