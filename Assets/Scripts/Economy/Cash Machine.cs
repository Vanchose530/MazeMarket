using SpriteGlow;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashMachine : MonoBehaviour, IInteractable
{
    public List<Item> containedItems { get; private set; }

    private bool _isActivated = false;

    [Header("Loot Options")]
    [SerializeField] private bool _isMoneyCointains = true;
    [SerializeField] private int _maxMoneyDrop = 100;
    [SerializeField] private int _minMoneyDrop = 0;
    
    [SerializeField] private bool _isAmmoContains = true;
    [SerializeField] private int _maxAmmoDrop = 30;
    [SerializeField] private int _minAmmoDrop = 0;

    
    [SerializeField] private bool _isBottleCointains = true;
    [SerializeField] private int _maxBottleDrop = 100;
    [SerializeField] private int _minBottleDrop = 0;

    [Header("Chances")]
    [Range(0, 100)]
    [SerializeField] private int _chanseOfMoney = 100;
    [Range(0, 100)]
    [SerializeField] private int _chanseOfAmmo = 20;
    [Range(0, 100)]
    [SerializeField] private int _chanseOfBottle = 5;

    [Header("Can Interact Effect")]
    [Range(0f, 10f)]
    [SerializeField] private float glowBrightnessCI;
    [Range(0, 10)]
    [SerializeField] private int outlineWidthCI;
    [Range(0f, 1f)]
    [SerializeField] private float alphaTreshHoldCI;

    // Base glow effect
    float glowBrightnessBase;
    int outlineWidthBase;
    float alphaTreshHoldBase;

    [Header("Sound Effect")]
    [SerializeField] private SoundEffect saveSE;

    [Header("Setup")]
    [SerializeField] SpriteGlowEffect spriteGlowEffect;

    private void GenerateLoot()
    {
        int dice = Random.Range(0, 100);
        containedItems = new List<Item>();
        if (_isMoneyCointains)
            if (dice < _chanseOfMoney)
            {
                Money newMoney = Instantiate(new GameObject()).AddComponent<Money>();
                newMoney._value = Random.Range(_minMoneyDrop, _maxMoneyDrop);
                containedItems.Add(newMoney);
            }
        if (_isAmmoContains)
            if (dice < _chanseOfAmmo)
            {
                AmmoItem newAmmo = Instantiate(new GameObject()).AddComponent<AmmoItem>();
                newAmmo.ammoCount = Random.Range(_minAmmoDrop, _maxAmmoDrop);
                newAmmo.ammoType = (AmmoTypes) Random.Range(0, 3 + 1);
                containedItems.Add(newAmmo);
            }
        /*if (_isBottleCointains)
            if(dice < _chanseOfBottle)
                containedItems.Add(new BottleItem(Random.Range(_minBottleDrop,_maxBottleDrop)); */
        // TODO: Дописать, когда класс БоттлИтем будет существовать
    }
    // TODO: Фикс глоу лайта
    public void CanInteract(Player player)
    {
        spriteGlowEffect.GlowBrightness = glowBrightnessBase;
        spriteGlowEffect.OutlineWidth = outlineWidthBase;
        spriteGlowEffect.AlphaThreshold = alphaTreshHoldBase;

    }

    public void CanNotInteract(Player player)
    {
        spriteGlowEffect.GlowBrightness = glowBrightnessCI;
        spriteGlowEffect.OutlineWidth = outlineWidthCI;
        spriteGlowEffect.AlphaThreshold = alphaTreshHoldCI;
    }

    public void Interact(Player player)
    {
        if (!_isActivated)
        {
            foreach (var item in containedItems)
            {
                item.PickUp();
            }
            _isActivated = true;
        }
        
    }

    private void OnValidate()
    {
        if (spriteGlowEffect != null)
            spriteGlowEffect = GetComponent<SpriteGlowEffect>();
    }

    private void Start()
    {
        glowBrightnessBase = spriteGlowEffect.GlowBrightness;
        outlineWidthBase = spriteGlowEffect.OutlineWidth;
        alphaTreshHoldBase = spriteGlowEffect.AlphaThreshold;

        GenerateLoot();
        Debug.Log("loot generated");
    }



}
