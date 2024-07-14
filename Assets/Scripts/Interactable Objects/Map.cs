using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour, IInteractable
{
    [Header("Interact Effect")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [SerializeField] private SoundEffect interactSE;

    private void OnValidate()
    {
        if (interactSpriteGlow == null)
            interactSpriteGlow = GetComponent<SpriteGlowEffect>();
    }

    private void Start()
    {
        interactSpriteGlow.enabled = false;
    }

    public void CanInteract(Player player)
    {
        interactSpriteGlow.enabled = true;
    }

    public void CanNotInteract(Player player)
    {
        interactSpriteGlow.enabled = false;

        if (MapUIM.instance.mapEnable)
            MapUIM.instance.HideMap();
    }

    public void Interact(Player player)
    {
        Debug.Log("Я карта");

        if (MapUIM.instance.mapEnable)
            MapUIM.instance.HideMap();
        else
            MapUIM.instance.ShowMap();
    }
}
