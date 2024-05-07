using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningManager : MonoBehaviour
{
    static LearningManager _instance;
    public static LearningManager instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<GameObject>(PATH_TO_SINGLETON_PREFAB);
                var inScene = Instantiate<GameObject>(prefab);
                _instance = inScene.GetComponentInChildren<LearningManager>();

                if (_instance == null)
                    _instance = inScene.AddComponent<LearningManager>();

                // DontDestroyOnLoad(_instance.transform.root.gameObject);
            }
            return _instance;
        }
    }

    const string PATH_TO_SINGLETON_PREFAB = "Singletons\\Learning Manager";

    [Header("Text Typing")]
    [SerializeField] private float timeToTypeOneLetter = 0.05f;
    [SerializeField] private SoundEffect typeSound;
    bool typing;

    public bool onLearning { get; private set; }

    private Queue<string> sentences;

    string currentSentence;

    private void Start()
    {
        typing = false;
    }

    public void StartLearning(LearningText text)
    {
        LearningUIM.instance.enablePanel = true;

        if (sentences == null) // если создавать очередь в старте случается баг
            sentences = new Queue<string>();

        sentences.Clear();

        foreach (string sentence in text.sentences)
        {
            sentences.Enqueue(sentence);
        }

        onLearning = true;

        GameEventsManager.instance.input.onInteractPressed += ProgressLearning;
        Time.timeScale = 0f;

        ProgressLearning();
    }

    public void ProgressLearning()
    {
        if (typing)
        {
            TypeAllSentence(currentSentence);
            return;
        }

        if (sentences.Count == 0)
        {
            EndLearning();
            return;
        }

        currentSentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    public void EndLearning()
    {
        LearningUIM.instance.learningTMP.text = "";
        LearningUIM.instance.enableCanProgressLearningSymbol = false;
        LearningUIM.instance.enablePanel = false;

        GameEventsManager.instance.input.onInteractPressed -= ProgressLearning;
        Time.timeScale = 1f;

        onLearning = false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        LearningUIM.instance.learningTMP.text = "";

        typing = true;
        LearningUIM.instance.enableCanProgressLearningSymbol = false;

        foreach (char letter in sentence.ToCharArray())
        {
            AudioManager.instance.PlaySoundEffect(typeSound, timeToTypeOneLetter);
            LearningUIM.instance.learningTMP.text += letter;
            yield return new WaitForSecondsRealtime(timeToTypeOneLetter);
        }

        LearningUIM.instance.enableCanProgressLearningSymbol = true;
        typing = false;
    }

    void TypeAllSentence(string sentence)
    {
        StopAllCoroutines();
        AudioManager.instance.PlaySoundEffect(typeSound, timeToTypeOneLetter);
        LearningUIM.instance.learningTMP.text = sentence;
        LearningUIM.instance.enableCanProgressLearningSymbol = true;
        typing = false;
    }
}
