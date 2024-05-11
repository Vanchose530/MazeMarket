using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IInteractable
{
    [Header("Color")]
    [SerializeField] private Color baseColor;
    [SerializeField] private Color interactColor;
    [SerializeField] private SpriteGlowEffect spriteGlow;
    

    [Header("Setup")]
    [SerializeField] private SpriteRenderer sp;

    bool canInteract = false;


    // Start is called before the first frame update
    void Start()
    {
        spriteGlow.EnableInstancing = true;
    }

    // Update is called once per frame
    
    public void CanInteract(Player player)
    {
        spriteGlow.EnableInstancing = false;
        canInteract = true;
    }

    public void CanNotInteract(Player player)
    {
        spriteGlow.EnableInstancing = true;
        canInteract = false;
        
    }

    public void Interact(Player player)
    {
        if (!player.isGrenade && !player.isEstos)
        {
            player.isEmptyBottle = false;
            player.isEstos = true;
            StartCoroutine(InteractAction());
        }
    }
    private IEnumerator InteractAction()
    {
        spriteGlow.GlowColor = interactColor;

        yield return new WaitForSecondsRealtime(0.5f);
        if (canInteract) {
            spriteGlow.GlowColor = baseColor;
            spriteGlow.EnableInstancing = false;
        }
        else
        {
            spriteGlow.GlowColor = baseColor;
            spriteGlow.EnableInstancing = true;
        }
    }
}
