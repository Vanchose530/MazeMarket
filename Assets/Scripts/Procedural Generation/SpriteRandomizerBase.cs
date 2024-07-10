using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizerBase : MonoBehaviour
{
    [Header("Sprites to Randomize")]
    [SerializeField] private List<Sprite> spritesToRandomize;
    [Header("Setup")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = spritesToRandomize[Random.Range(0, spritesToRandomize.Count)];
    }
}
