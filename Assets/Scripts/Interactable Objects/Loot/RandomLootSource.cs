using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLootSource : MonoBehaviour, IInteractable
{
    [Header("Loot")]
    [SerializeField] private Balancer<LootObject> lootPool;
    private LootObject loot;

    [Header("Setup")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteGlowEffect glow;

    [Header("SFX")]
    [SerializeField] private SoundEffect openSound;
    [SerializeField] private SoundEffect cantOpenSound;

    bool opened;

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (glow == null)
            glow = GetComponent<SpriteGlowEffect>();
    }

    private void Awake()
    {
        glow.enabled = false;
        opened = false;
    }

    public void CanInteract(Player player)
    {
        if (!opened)
            glow.enabled = true;
        else
            glow.enabled = false;
    }

    public void CanNotInteract(Player player)
    {
        glow.enabled = false;
    }

    public void Interact(Player player)
    {
        if (!opened)
        {
            if (loot == null)
                loot = lootPool.Get();

            if (loot.GetCanLoot())
            {
                loot.Loot();
                opened = true;
                glow.enabled = false;

                if (animator != null)
                    animator.Play("Loot");

                if (openSound != null)
                    AudioManager.instance.PlaySoundEffect(openSound);
            }
            else
            {
                Debug.Log("Cant loot this source!");

                if (cantOpenSound != null)
                    AudioManager.instance.PlaySoundEffect(cantOpenSound);
                // нужна индикация того, что источник нельзя залутать
            }
        }

    }
}
