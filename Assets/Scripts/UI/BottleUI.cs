using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BottleUI : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private Sprite emptyBottle;
    [SerializeField] private Sprite grenade;
    [SerializeField] private Sprite healthBottle;
    [Header("Setup")]
    [SerializeField] private UnityEngine.UI.Image image;

    private void OnValidate()
    {
        if(image == null)
            image = GetComponent<UnityEngine.UI.Image>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.isEmptyBottle)
            image.sprite = emptyBottle;
        if (Player.instance.isGrenade)
            image.sprite = grenade;
        if (Player.instance.isEstos)
            image.sprite = healthBottle;
    }
}
