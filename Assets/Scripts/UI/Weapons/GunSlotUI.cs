using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSlotUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Image image;
    public Gun gun { get; private set; }

    public void SetGun(Gun gun)
    {
        image.enabled = true;
        image.sprite = gun.image;
        this.gun = gun;
    }

    public void Clear()
    {
        image.enabled = false;
        image.sprite = null;
        gun = null;
    }
}
