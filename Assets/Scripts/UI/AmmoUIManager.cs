using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : MonoBehaviour
{
    public static AmmoUIManager instance { get; private set; }

    [Header("General")]
    [SerializeField] private GameObject ammoPanel;
    public bool ammoPanelActive { get { return ammoPanel.active; } set { ammoPanel.SetActive(value); } }
    [SerializeField] private Animator _ammoPanelAnimator;
    public Animator ammoPanelAnimator { get {  return _ammoPanelAnimator; } }
    [SerializeField] private TextMeshProUGUI allAmmo;
    public string allAmmoText { get { return allAmmo.text; } set { allAmmo.text = value; } }
    [SerializeField] private TextMeshProUGUI ammoInGun;
    public string ammoInGunText { get { return ammoInGun.text; } set { ammoInGun.text = value; } }
    [SerializeField] private Image ammoImage;
    public AmmoTypes ammoType
    {
        set
        {
            switch (value)
            {
                case AmmoTypes.LightBullets:
                    ammoImage.sprite = lightBulletsImage;
                    break;
                case AmmoTypes.MediumBullets:
                    ammoImage.sprite = mediumBulletsImage;
                    break;
                case AmmoTypes.HeavyBullets:
                    ammoImage.sprite = heavyBulletsImage;
                    break;
                case AmmoTypes.Shells:
                    ammoImage.sprite = shellsImage;
                    break;
            }
        }
    }

    [Header("Ammo Images")]
    [SerializeField] private Sprite lightBulletsImage;
    [SerializeField] private Sprite mediumBulletsImage;
    [SerializeField] private Sprite heavyBulletsImage;
    [SerializeField] private Sprite shellsImage;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Ammo UI Manager in scene");
        instance = this;
    }
}
