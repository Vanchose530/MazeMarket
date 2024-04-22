using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneInstaller : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private GameObject gameplayCanvas;

    private void Awake()
    {
        AudioManager.instance.SetMusic(levelMusic);
        AudioManager.instance.normalSnapshot.TransitionTo(0.1f);
        CursorManager.instance.aimVisible = true;

        if (gameplayCanvas.GetComponent<Canvas>() != null)
        {
            Instantiate(gameplayCanvas);
        }
        else
        {
            Debug.LogError("Gamplay Canvas prefab doesn't contains Canvas");
        }
    }
}
