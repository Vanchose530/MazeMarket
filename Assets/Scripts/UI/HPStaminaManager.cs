using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPStaminaManager : MonoBehaviour
{
    public static HPStaminaManager instance { get; private set; }

    [Header("HP")]
    [SerializeField] private Slider _hpSlider;
    public Slider hpSlider { get { return _hpSlider; } }

    [Header("Stamina")]
    [SerializeField] private Slider _staminaSlider;
    public Slider staminaSlider { get { return _staminaSlider; } }

    [SerializeField] private Animator staminaSliderAnimator;

    private bool _canUseStamina;
    public bool canUseStamina
    {
        get { return _canUseStamina; }
        set
        {
            if (value)
            {
                staminaSliderAnimator.Play("CanUse");
            }
            else
            {
                staminaSliderAnimator.Play("CantUse");
            }
            _canUseStamina = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one HPStamina Manager in scene");
        instance = this;
    }
}
