using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFountain : MonoBehaviour,IInteractable
{
    [Header("Color")]
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Color canInteractColor;
    [SerializeField] private Color interactColor;
    [SerializeField] private Color emptyFountain;
    private Color baseColor;

    bool canInteract = false;
    bool isEmpty = false;
    bool isFull = true;

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
        if (isFull && !player.isEstos)
        {
            player.isEmptyBottle = false;
            player.isGrenade = true;
            StartCoroutine(InteractAction());
            isFull = false;
        }
    }

    public void CanInteract(Player player)
    {
        sp.color = canInteractColor;
        canInteract = true;
    }

    public void CanNotInteract(Player player)
    {
        if (isEmpty)
        {
            sp.color = emptyFountain;
            canInteract = false;
        }
        else {
            sp.color = baseColor;
            canInteract = false;
        }
        
    }

    private IEnumerator InteractAction()
    {
        sp.color = interactColor;
        
        yield return new WaitForSecondsRealtime(0.5f);
        if (canInteract)
            sp.color = canInteractColor;
        else {
            sp.color = emptyFountain;
            isEmpty = true;
        }
            
    }
    
}
