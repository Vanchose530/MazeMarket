using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CultistsPot : MonoBehaviour, IInteractable
{
    [Header("Sprites")]
    // [SerializeField] private Sprite fullOfDemonsBloodPotSprite;
    [SerializeField] private Sprite voidPotSprite;

    [Header("Interact Effect")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [SerializeField] private SoundEffect interactSE;

    [Header("Setup")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D demonsBloodLight;

    const float HINT_TIME = 2.5f;

    private void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (interactSpriteGlow == null)
            interactSpriteGlow = GetComponent<SpriteGlowEffect>();
        if (demonsBloodLight == null)
            demonsBloodLight = GetComponentInChildren<Light2D>();
    }

    private void OnDestroy()
    {
        interactSpriteGlow.enabled = false;
        spriteRenderer.sprite = voidPotSprite;
        demonsBloodLight.enabled = false;
    }

    void Start()
    {
        interactSpriteGlow.enabled = false;
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
        if (PlayerInventory.instance.countEmptyBottle > 0)
        {
            PlayerInventory.instance.countGrenadeBottle++;
            PlayerInventory.instance.countEmptyBottle--;

            HintsManager.instance.ShowPleasureNotice("Добавлено граната!", HINT_TIME);

            if (interactSE != null)
                AudioManager.instance.PlaySoundEffect(interactSE, transform.position);

            Destroy(this);
        }
        else
        {
            HintsManager.instance.ShowDefaultNotice("Нет пустых банок!", HINT_TIME);
        }

    }
}
