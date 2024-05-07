using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LearningPoint : MonoBehaviour, IInteractable
{
    [Header("Text")]
    [SerializeField] private LearningText learningText;

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

    [Header("Setup")]
    [SerializeField] SpriteGlowEffect spriteGlowEffect;

    private bool canStartLearning;

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

        canStartLearning = true;
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

        canStartLearning = true;
    }

    public void Interact(Player player)
    {
        if (!LearningManager.instance.onLearning && canStartLearning)
        {
            LearningManager.instance.StartLearning(learningText);
            canStartLearning = false;
        }
    }
}
