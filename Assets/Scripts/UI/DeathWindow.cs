using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWindow : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject deathPanel;

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ShowDeathWindow()
    {
        active = true;
        // AudioManager.instance.onDeathSnapshot.TransitionTo(0.1f);
        PlayerConditionsManager.instance.currentCondition = PlayerConditions.Death;
        deathPanel.SetActive(true);
    }
}
