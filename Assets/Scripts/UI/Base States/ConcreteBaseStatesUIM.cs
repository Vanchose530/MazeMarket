using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI hpTMP;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private TextMeshProUGUI staminaTMP;

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

        if (hpTMP != null)
        {
            hpTMP.text = $"{(int)currentHealth} / {(int)maxHealth}";
        }
    }

    void UpdateStaminaSlider()
    {
        if (currentStamina > 0)
            staminaSlider.value = currentStamina / maxStamina;
        else
            staminaSlider.value = 0;

        if (staminaTMP != null)
        {
            staminaTMP.text = $"{(int)currentStamina} / {(int)maxStamina}";
        }
    }
}
