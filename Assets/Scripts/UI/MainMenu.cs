using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstChoice;
    [SerializeField] private string levelToLoadName;

    private void Start()
    {
        if (firstChoice != null)
            StartCoroutine("SelectFirstChoice");

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void PlayButtonPressed()
    {
        PlayerDataKeeper.instance.ClearData();
        SceneManager.LoadScene(levelToLoadName);
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
