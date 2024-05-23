using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponNameTMP;

    const string PATH_TO_PNGS = "Weapons\\Pngs\\";

    private Weapon _weapon;
    public Weapon weapon
    {
        get { return _weapon; }
        set
        {
            full = value != null;

            _weapon = value;

            if (value != null)
            {
                SetWeaponInSlot(); 
                CheckForChosen();
            }
        }
    }

    private bool _full;
    public bool full
    {
        get { return _full; }
        private set
        {
            animator.SetBool("Full", value);
            _full = value;
        }
    }

    private bool _chosen;
    public bool chosen
    {
        get { return _chosen; }
        set
        {
            animator.SetBool("Chosen", value);
            _chosen = value;
        }
    }

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetBool("Full", full);
        animator.SetBool("Chosen", chosen);
    }

    public void CheckForChosen()
    {
        chosen = weapon == PlayerWeaponsManager.instance.currentWeapon; 
    }

    private void SetWeaponInSlot()
    {
        /*if ((weapon.name == "AK-47(Clone)") || (weapon.name == "Pump(Clone)"))
            weaponImage.sprite = Resources.Load<Sprite>(PATH_TO_PNGS + weapon.displayName.Replace("(Clone)", ""));
        else weaponImage.sprite = weapon.image;*/
        weaponNameTMP.text = weapon.displayName;
        weaponImage.sprite = weapon.image;
    }
}
