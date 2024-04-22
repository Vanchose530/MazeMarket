using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneInstaller : MonoBehaviour
{
    public AudioClip menuMusic;

    private void Awake()
    {
        AudioManager.instance.SetMusic(menuMusic);
        AudioManager.instance.normalSnapshot.TransitionTo(0.1f);
    }
}
