using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcreteBaseStatesUIM : BaseStatesUIM
{
    float maxHealth;
    float currentHealth;

    float maxStamina;
    float currentStamina;

    [Header("Setup")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider staminaSlider;

    [Header("Animations")]
    [SerializeField] private Animator staminaSliderAnimator;

    private void OnValidate()
    {
        if (staminaSlider != null && staminaSliderAnimator == null)
            staminaSliderAnimator = staminaSlider.GetComponent<Animator>();
    }

    public override void SetCurrentHealth(int health)
    {
        currentHealth = health;
        UpdateHPSlider();
    }

    public override void SetCurrentStamina(float stamina)
    {
        currentStamina = stamina;
        UpdateStaminaSlider();
    }

    public override void SetMaxHealth(int health)
    {
        maxHealth = health;
        UpdateHPSlider();
    }

    public override void SetMaxStamina(float stamina)
    {
        maxStamina = stamina;
        UpdateStaminaSlider();
    }

    public override void SetCanUseStamina(bool canUseStamina)
    {
        if (canUseStamina)
            staminaSliderAnimator.Play("CanUse");
        else
            staminaSliderAnimator.Play("CantUse");
    }

    void UpdateHPSlider()
    {
        if (currentHealth > 0)
            hpSlider.value = currentHealth / maxHealth;
        else
            hpSlider.value = 0;
    }

    void UpdateStaminaSlider()
    {
        if (currentStamina > 0)
            staminaSlider.value = currentStamina / maxStamina;
        else
            staminaSlider.value = 0;
    }
}
