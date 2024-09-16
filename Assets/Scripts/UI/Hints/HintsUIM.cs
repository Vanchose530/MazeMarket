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

    [SerializeField] private GameObject dropHint;
    [SerializeField] private Animator dropHintAnimator;
    [SerializeField] private float dropHintTime = 4f;

    [Header("Notices")]
    [SerializeField] private TextHint _defaultNotice;
    public TextHint defaultNotice { get { return _defaultNotice; } }
    [SerializeField] private TextHint _pleasureNotice;
    public TextHint pleasureNotice { get { return _pleasureNotice; } }
    [SerializeField] private TextHint _warningNotice;
    public TextHint warningNotice { get { return _warningNotice; } }

    [Header("Loot")]
    [SerializeField] private TextHint _lootHint;
    public TextHint lootHint { get { return _lootHint; } }

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
        dropHintAnimator.Play("Hide");
    }

    public void ShowSaveHint() => StartCoroutine(SaveHint(saveHintTime));

    IEnumerator SaveHint(float time)
    {
        saveHintAnimator.Play("Show");
        yield return new WaitForSecondsRealtime(time);
        saveHintAnimator.Play("Hide");
    }

    public void ShowDropHint() => StartCoroutine(CanNotDropItem(saveHintTime));
    IEnumerator CanNotDropItem(float time)
    {
        dropHintAnimator.Play("Show");
        yield return new WaitForSecondsRealtime(time);
        dropHintAnimator.Play("Hide");
    }
}
