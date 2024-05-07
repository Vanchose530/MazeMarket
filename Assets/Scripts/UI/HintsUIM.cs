using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsUIM : MonoBehaviour
{
    public static HintsUIM instance { get; private set; }

    [Header("Hints")]
    [SerializeField] private GameObject interactHint;
    public bool enableInteractHint
    {
        get { return interactHint.active; }
        set { interactHint.SetActive(value); }
    }

    [SerializeField] private Animator saveHintAnimator;
    [SerializeField] private float saveHintTime = 4f;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Hints UI Manager in scene");
        instance = this;
    }

    private void Start()
    {
        enableInteractHint = false;
        saveHintAnimator.Play("Hide");
    }

    public void ShowSaveHint() => StartCoroutine(SaveHint(saveHintTime));

    IEnumerator SaveHint(float time)
    {
        saveHintAnimator.Play("Show");
        yield return new WaitForSecondsRealtime(time);
        saveHintAnimator.Play("Hide");
    }
}
