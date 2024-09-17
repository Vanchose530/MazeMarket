using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextHint : MonoBehaviour
{
    [Header("Text Hint")]
    [SerializeField] private TextMeshProUGUI textTMP;
    public string text { get { return textTMP.text; } }

    [SerializeField] private Animator animator;

    const string ANIMATION_SHOW_TEXT = "Show";
    const string ANIMATION_HIDE_TEXT = "Hide";

    public bool visible { get; private set; }

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
        visible = true;
    }

    public void Hide()
    {
        textTMP.text = "none";
        animator?.Play(ANIMATION_HIDE_TEXT);
        visible = false;
    }
}
