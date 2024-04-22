using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<GameObject>(PATH_TO_SINGLETON_PREFAB);
                var inScene = Instantiate<GameObject>(prefab);
                _instance = inScene.GetComponentInChildren<AudioManager>();

                if (_instance == null)
                    _instance = inScene.AddComponent<AudioManager>();

                DontDestroyOnLoad(_instance.transform.root.gameObject);
            }

            return _instance;
        }
    }

    const string PATH_TO_SINGLETON_PREFAB = "Singletons\\Audio Manager";

    [SerializeField] private AudioSource currentLevelMusic;
    [SerializeField] private AudioMixerGroup mixerGroupSFX;

    [Header("Music Snapshots")]
    [SerializeField] private AudioMixerSnapshot _normalSnapshot;
    public AudioMixerSnapshot normalSnapshot { get { return _normalSnapshot; } }
    [SerializeField] private AudioMixerSnapshot _inMenuSnapshot;
    public AudioMixerSnapshot inMenuSnapshot { get { return _inMenuSnapshot; } }
    [SerializeField] private AudioMixerSnapshot _onDeathSnapshot;
    public AudioMixerSnapshot onDeathSnapshot { get { return _onDeathSnapshot; } }
    [SerializeField] private AudioMixerSnapshot _battleSnapshot;
    public AudioMixerSnapshot battleSnapshot { get { return _battleSnapshot; } }

    public void PlaySoundEffect(SoundEffect soundEffect, float soundEffectExistTime = 20f)
    {
        var audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.clip = soundEffect.sound;
        audioSource.pitch = Random.Range(soundEffect.minPitch, soundEffect.maxPitch);
        audioSource.volume = soundEffect.volume;
        audioSource.outputAudioMixerGroup = mixerGroupSFX;

        var effect = Instantiate(audioSource);
        Destroy(effect, soundEffectExistTime);
    }

    public void PlaySoundEffect(SoundEffect soundEffect, Vector2 soundEffectPosition, float soundEffectExistTime = 20f)
    {
        var audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.clip = soundEffect.sound;
        audioSource.pitch = Random.Range(soundEffect.minPitch, soundEffect.maxPitch);
        audioSource.volume = soundEffect.volume;
        audioSource.outputAudioMixerGroup = mixerGroupSFX;

        var effect = Instantiate(audioSource, soundEffectPosition, Quaternion.identity);
        Destroy(effect, soundEffectExistTime);
    }

    public AudioSource GetSoundEffectAS(SoundEffect soundEffect)
    {
        var audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.clip = soundEffect.sound;
        audioSource.pitch = Random.Range(soundEffect.minPitch, soundEffect.maxPitch);
        audioSource.volume = soundEffect.volume;
        audioSource.outputAudioMixerGroup = mixerGroupSFX;

        var effect = Instantiate(audioSource);

        return effect;
    }

    public AudioSource GetSoundEffectAS(SoundEffect soundEffect, Vector2 soundEffectPosition)
    {
        var audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.clip = soundEffect.sound;
        audioSource.pitch = Random.Range(soundEffect.minPitch, soundEffect.maxPitch);
        audioSource.volume = soundEffect.volume;
        audioSource.outputAudioMixerGroup = mixerGroupSFX;

        var effect = Instantiate(audioSource, soundEffectPosition, Quaternion.identity);

        return effect;
    }

    public void SetMusic(AudioClip music, bool restartIfMatch = false)
    {
        if (restartIfMatch || currentLevelMusic.clip != music)
        {
            currentLevelMusic.clip = music;
            currentLevelMusic.Play();
        }
    }
}
