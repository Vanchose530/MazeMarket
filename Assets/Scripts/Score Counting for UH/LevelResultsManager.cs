using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelResultsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI defeatedEnemyesCountTMP;
    [SerializeField] private TextMeshProUGUI levelTimeTMP;

    private void Start()
    {
        defeatedEnemyesCountTMP.text = System.Convert.ToString(SimpleScoreCounter.instance.defeatedEnemyesCount);
        SimpleScoreCounter.instance.countLevelTime = false;
        System.TimeSpan ts = System.TimeSpan.FromSeconds((int)SimpleScoreCounter.instance.levelTime);
        levelTimeTMP.text = System.Convert.ToString(ts.ToString());
    }

    private void Update()
    {
        if (InputManager.instance.GetInteractPressed())
        {
            SimpleScoreCounter.instance.Reset();
            SceneManager.LoadScene("Main Menu");
        }
    }
}
