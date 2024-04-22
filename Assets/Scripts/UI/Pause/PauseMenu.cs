using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }

    [Header("General")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject firstChoice;

    bool pause;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Pause Menu script in scene");
        instance = this; 
    }

    private void Start()
    {
        Pause(false);
    }

    private void OnEnable()
    {
        Invoke("OnEnableAfterTime", 0.1f);
    }

    private void OnEnableAfterTime()
    {
        GameEventsManager.instance.input.onPausePressed += SetOnOffPause;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.input.onPausePressed -= SetOnOffPause;
        Pause(false);
    }

    public void ResumeButtonPressed()
    {
        Pause(false);
    }

    public void OptionsButtonPressed()
    {
        Debug.Log("Options");
    }

    public void MainMenuButtonPressed()
    {
        Debug.Log("Quit");
        SceneManager.LoadScene("Main Menu");
    }

    private void Pause(bool pause)
    {
        if (pause)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            StartCoroutine("SelectFirstChoice");
            GameEventsManager.instance.pause.PauseIn();
            AudioManager.instance.inMenuSnapshot.TransitionTo(0.5f);
        }
        else
        {
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            GameEventsManager.instance.pause.PauseOut();

            if (Player.instance.isOnBattle)
                AudioManager.instance.battleSnapshot.TransitionTo(0.5f);
            else
                AudioManager.instance.normalSnapshot.TransitionTo(0.5f);
        }

        this.pause = pause;
    }

    private void SetOnOffPause()
    {
        if (Player.instance == null)
            return;
        Pause(!pause);
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstChoice);
    }
}
