using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWindow : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject deathPanel;

    [Header("Score Counting")]
    [SerializeField] private TextMeshProUGUI defeatedEnemyesCountTMP;
    [SerializeField] private TextMeshProUGUI levelTimeTMP;

    bool active;

    private void Start()
    {
        active = false;
        deathPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Invoke("OnEnableAfterTime", 0.1f);
    }

    private void OnEnableAfterTime()
    {
        GameEventsManager.instance.player.onPlayerDeath += ShowDeathWindow;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.player.onPlayerDeath -= ShowDeathWindow;
    }

    private void Update()
    {
        if (active && InputManager.instance.GetInteractPressed())
        {
            SimpleScoreCounter.instance.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ShowDeathWindow()
    {
        active = true;

        defeatedEnemyesCountTMP.text = System.Convert.ToString(SimpleScoreCounter.instance.defeatedEnemyesCount);
        SimpleScoreCounter.instance.countLevelTime = false;
        System.TimeSpan ts = System.TimeSpan.FromSeconds((int)SimpleScoreCounter.instance.levelTime);
        levelTimeTMP.text = System.Convert.ToString(ts.ToString());

        AudioManager.instance.onDeathSnapshot.TransitionTo(0.1f);
        deathPanel.SetActive(true);
    }
}
