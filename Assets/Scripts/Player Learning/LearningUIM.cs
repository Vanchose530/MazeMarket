using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LearningUIM : MonoBehaviour
{
    public static LearningUIM instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private GameObject learningPanel;
    public bool enablePanel
    {
        get { return learningPanel.active; }
        set { learningPanel.active = value; }
    }

    [SerializeField] private TextMeshProUGUI _learningTMP;
    public TextMeshProUGUI learningTMP { get { return _learningTMP; } }

    [SerializeField] private GameObject canProgressLearningSymbol;
    public bool enableCanProgressLearningSymbol
    {
        get { return canProgressLearningSymbol.active; }
        set { canProgressLearningSymbol.active = value; }
    }



    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Learning UI Manager in scene");
        instance = this;
    }

    private void Start()
    {
        enablePanel = false;
        enableCanProgressLearningSymbol = false;
        learningTMP.text = "";
    }
}
