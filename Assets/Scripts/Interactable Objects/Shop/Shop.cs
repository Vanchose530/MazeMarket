using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{

    [Header("Interact Effect")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [SerializeField] private SoundEffect interactSE;

    void Start()
    {
        interactSpriteGlow.enabled = false;
    }

    public void CanInteract(Player player)
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
        {
            return;
        }

        interactSpriteGlow.enabled = true;
    }

    public void CanNotInteract(Player player)
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
        {
            return;
        }

        interactSpriteGlow.enabled = false;
    }

    public void Interact(Player player)
    {
        if (PlayerConditionsManager.instance.currentCondition == PlayerConditions.Battle)
        {
            return;
        }
    }
}
