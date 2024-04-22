using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstChoice;

    private void Start()
    {
        StartCoroutine("SelectFirstChoice");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayButtonPressed()
    {
        SceneManager.LoadScene("Level 1.1");
    }

    public void OptionsButtonPressed()
    {
        Debug.Log("Options");
    }

    public void QuitButtonPressed()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstChoice);
    }
}
