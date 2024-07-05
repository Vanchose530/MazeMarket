using SpriteGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    //Базовый магаз - зашел увидел купил сколько надо и ушел. В каждом маге 3-5 "позиций"


    public List<Item> containedItems { get; private set; }



    [Header("Shop Options")]
    [SerializeField] private int _maxPositions = 5;
    [SerializeField] private int _minPositions = 3;

    [SerializeField] private bool _isWeaponCointains = true;
    [SerializeField] private int _maxWeaponDrop = 3;
    [SerializeField] private int _minWeaponDrop = 0;

    [SerializeField] private bool _isAmmoContains = true;
    [SerializeField] private int _maxAmmoDrop = 30;
    [SerializeField] private int _minAmmoDrop = 0;


    [SerializeField] private bool _isBottleCointains = true;
    [SerializeField] private int _maxBottleDrop = 100;
    [SerializeField] private int _minBottleDrop = 0;

    [Header("Chances of spawn")]
    [Range(0, 100)]
    [SerializeField] private int _chanseOfAmmo = 35;
    [Range(0, 100)]
    [SerializeField] private int _chanseOfBottle = 10;
    [Range(0, 100)]
    [SerializeField] private int _chanseOfWeapon = 5;

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

        if (_isWeaponCointains)
            //if (dice < _chanseOfWeapon)
              //  int diceWeaponCounter = Random.Range(0, 100)
                //containedItems.Add(new Money(Random.Range(_minWeaponDrop, _maxWeaponDrop)));
        if (_isAmmoContains)
            if (dice < _chanseOfAmmo) 
                {
                    int diceWeaponCounter = Random.Range(0, 100);
                containedItems.Add(new AmmoItem(Random.Range(0, 3 + 1), Random.Range(_minAmmoDrop, _maxAmmoDrop)));
                }
        /*if (_isBottleCointains)
            if(dice < _chanseOfBottle)
                containedItems.Add(new BottleItem(Random.Range(_minBottleDrop,_maxBottleDrop)); */
        // TODO: Дописать, когда класс БоттлИтем будет существовать
    }
    // TODO: Фикс глоу лайта
    public void CanInteract(Player player)
    {
        //spriteGlowEffect.GlowBrightness = glowBrightnessBase;
        //spriteGlowEffect.OutlineWidth = outlineWidthBase;
        //spriteGlowEffect.AlphaThreshold = alphaTreshHoldBase;

    }

    public void CanNotInteract(Player player)
    {
        //spriteGlowEffect.GlowBrightness = glowBrightnessCI;
        //spriteGlowEffect.OutlineWidth = outlineWidthCI;
        //spriteGlowEffect.AlphaThreshold = alphaTreshHoldCI;
    }

    public void Interact(Player player)
    {


    }

    private void OnValidate()
    {
        if (spriteGlowEffect != null)
            spriteGlowEffect = GetComponent<SpriteGlowEffect>();
    }

    private void Start()
    {
        //glowBrightnessBase = spriteGlowEffect.GlowBrightness;
        //outlineWidthBase = spriteGlowEffect.OutlineWidth;
        //alphaTreshHoldBase = spriteGlowEffect.AlphaThreshold;

        GenerateLoot();

    }

}
