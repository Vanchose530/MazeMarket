using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPStaminaManager : MonoBehaviour
{
    public static HPStaminaManager instance { get; private set; }

    [SerializeField] private Slider _hpSlider;
    public Slider hpSlider { get { return _hpSlider; } }

    [SerializeField] private Slider _staminaSlider;
    public Slider staminaSlider { get { return _staminaSlider; } }

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one HPStamina Manager in scene");
        instance = this;
    }
}
