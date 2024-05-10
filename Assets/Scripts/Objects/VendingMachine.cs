using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IInteractable
{
    [Header("Color")]
    [SerializeField] private Color canInteractColor;
    [SerializeField] private Color interactColor;
    [SerializeField] private Color emptyVendingMachine;
    private Color baseColor;

    [Header("Setup")]
    [SerializeField] private SpriteRenderer sp;

    bool canInteract = false;


    // Start is called before the first frame update
    private void OnValidate()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        baseColor = new Color(sp.color.r, sp.color.g, sp.color.b);
    }

    // Update is called once per frame
    
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
        sp.color = interactColor;

        yield return new WaitForSecondsRealtime(0.5f);
        if (canInteract)
            sp.color = canInteractColor;

    }
}
