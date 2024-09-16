using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHint : MonoBehaviour
{
    [Header("Text Hint")]
    [SerializeField] private TextMeshProUGUI textTMP;
    [SerializeField] private Animator animator;

    const string ANIMATION_SHOW_TEXT = "Show";
    const string ANIMATION_HIDE_TEXT = "Hide";

    private void OnValidate()
    {
        if (textTMP == null)
            textTMP = GetComponent<TextMeshProUGUI>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Show(string text)
    {
        textTMP.text = text;
        animator.Play(ANIMATION_SHOW_TEXT);
    }

    public void Hide()
    {
        textTMP.text = "none";
        animator?.Play(ANIMATION_HIDE_TEXT);
    }
}
