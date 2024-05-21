using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingToTheNextLevel : MonoBehaviour, IInteractable
{
    [Header("Next Level")]
    [SerializeField] private string nextLevelName;
    [SerializeField] private Vector2 nextLevelStartPosition;

    [Header("Can Interact")]
    [SerializeField] private GameObject canInteractIndicator;

    void Start()
    {
        canInteractIndicator.SetActive(false);
    }

    public void CanInteract(Player player)
    {
        canInteractIndicator.SetActive(true);
    }

    public void CanNotInteract(Player player)
    {
        canInteractIndicator.SetActive(false);
    }

    public void Interact(Player player)
    {
        // player.SavePlayerData(nextLevelStartPosition);
        SceneManager.LoadScene(nextLevelName);
    }
}
