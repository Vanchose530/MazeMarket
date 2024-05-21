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

    [SerializeField] private Animator reloadingHintAnimator;

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
        reloadingHintAnimator.Play("Hide");
    }

    private void Update()
    {
        // подобную логику не стоит реализовывать через update

        if (PlayerWeaponsManager.instance.currentGun != null
            && PlayerWeaponsManager.instance.currentGun.ammoInMagazine == 0
            && PlayerWeaponsManager.instance.GetAmmoByType(PlayerWeaponsManager.instance.currentGun.ammoType) != 0)
        {
            reloadingHintAnimator.Play("Show");
        }
        else
        {
            reloadingHintAnimator.Play("Hide");
        }
    }

    public void ShowSaveHint() => StartCoroutine(SaveHint(saveHintTime));

    IEnumerator SaveHint(float time)
    {
        saveHintAnimator.Play("Show");
        yield return new WaitForSecondsRealtime(time);
        saveHintAnimator.Play("Hide");
    }
}
