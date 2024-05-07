using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("Can Interact Effect")]
    [Range(0f, 10f)]
    [SerializeField] private float glowBrightnessCI;
    [Range(0, 10)]
    [SerializeField] private int outlineWidthCI;
    [Range(0f, 1f)]
    [SerializeField] private float alphaTreshHoldCI;

    // Base glow effect
    float glowBrightnessBase;
    int outlineWidthBase;
    float alphaTreshHoldBase;

    [Header("Sound Effect")]
    [SerializeField] private SoundEffect saveSE;

    [Header("Setup")]
    [SerializeField] SpriteGlowEffect spriteGlowEffect;

    bool canSave;

    private void OnValidate()
    {
        if (spriteGlowEffect != null)
            spriteGlowEffect = GetComponent<SpriteGlowEffect>();
    }

    private void Start()
    {
        glowBrightnessBase = spriteGlowEffect.GlowBrightness;
        outlineWidthBase = spriteGlowEffect.OutlineWidth;
        alphaTreshHoldBase = spriteGlowEffect.AlphaThreshold;

        canSave = true;
    }

    public void CanInteract(Player player)
    {
        spriteGlowEffect.GlowBrightness = glowBrightnessCI;
        spriteGlowEffect.OutlineWidth = outlineWidthCI;
        spriteGlowEffect.AlphaThreshold = alphaTreshHoldCI;
    }

    public void CanNotInteract(Player player)
    {
        spriteGlowEffect.GlowBrightness = glowBrightnessBase;
        spriteGlowEffect.OutlineWidth = outlineWidthBase;
        spriteGlowEffect.AlphaThreshold = alphaTreshHoldBase;

        canSave = true;
    }

    public void Interact(Player player)
    {
        if (canSave)
        {
            HintsUIM.instance.ShowSaveHint();
            AudioManager.instance.PlaySoundEffect(saveSE, 2.5f); // время за хардкожено

            player.levelName = SceneManager.GetActiveScene().name;
            player.startPosition = transform.position;
            DataPersistenceManager.instance.SaveGame();

            canSave = false;
        }
    }

}
