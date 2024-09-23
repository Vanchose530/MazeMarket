using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeaponSlotUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Image image;
    [SerializeField] private Sprite fistImage;
    public MeleeWeapon meleeWeapon { get; private set; }

    public void SetMeleeWeapon(MeleeWeapon meleeWeapon)
    {
        image.enabled = true;
        image.sprite = meleeWeapon.image;
        this.meleeWeapon = meleeWeapon;
    }

    public void Clear()
    {
        image.sprite = fistImage;
        meleeWeapon = null;
    }
}
