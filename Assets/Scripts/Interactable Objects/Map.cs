using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Map : MonoBehaviour, IInteractable
{
    [Header("Interact Effect")]
    [SerializeField] private SpriteGlowEffect interactSpriteGlow;
    [SerializeField] private SoundEffect interactSE;
    [SerializeField] private Light2D sourceLight;

    const float HINT_TIME = 2.5f;

    private void OnValidate()
    {
        if (interactSpriteGlow == null)
            interactSpriteGlow = GetComponent<SpriteGlowEffect>();
        if (sourceLight == null)
            sourceLight = GetComponentInChildren<Light2D>();
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
        //if (MapUIM.instance.mapEnable)
        //    MapUIM.instance.HideMap();
        //else
        //    MapUIM.instance.ShowMap();

        MiniMapUIM.instance.UseMap();
        HintsManager.instance.ShowPleasureNotice("Карта обновлена!", HINT_TIME);

        if (sourceLight != null)
            sourceLight.enabled = false;

        Destroy(this);
    }

    private void OnDestroy()
    {
        interactSpriteGlow.enabled = false;
        Player.instance.interactableObjectsDetector.StartRemoveFromInteracteble(this);
    }
}
