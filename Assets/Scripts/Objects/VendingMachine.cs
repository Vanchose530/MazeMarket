using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IInteractable
{
    [Header("Interact Cooldown")]
    [SerializeField] private float interactCooldown = 0.5f;

    [Header("Can not interact")]
    [SerializeField] private Color canNotInteractGlowColor;
    private Color glowBaseColor;

    [Header("Interact Effect")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [SerializeField] private SoundEffect interactSE;

    bool canInteract = false;

    private void OnValidate()
    {
        if (interactSpriteGlow == null)
            interactSpriteGlow = GetComponent<SpriteGlowEffect>();
    }

    void Start()
    {
        interactSpriteGlow.enabled = false;
        canInteract = true;

        glowBaseColor = interactSpriteGlow.GlowColor;
    }
    
    public void CanInteract(Player player)
    {
        interactSpriteGlow.enabled = true;
    }

    public void CanNotInteract(Player player)
    {
        interactSpriteGlow.enabled = false;
    }

    public void Interact(Player player)
    {
        if (canInteract && !player.isGrenade && !player.isEstos)
        {
            player.isEmptyBottle = false;
            player.isEstos = true;

            if (interactSE != null)
                AudioManager.instance.PlaySoundEffect(interactSE, transform.position);

            StartCoroutine(SetCooldown());
        }
    }
    private IEnumerator SetCooldown()
    {
        canInteract = false;
        interactSpriteGlow.GlowColor = canNotInteractGlowColor;

        yield return new WaitForSecondsRealtime(interactCooldown);

        interactSpriteGlow.GlowColor = glowBaseColor;
        canInteract = true;
    }
}
