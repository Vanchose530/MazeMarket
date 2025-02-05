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
    [SerializeField] private AudioMixer audioMixerMusic;
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

    private void Update()
    {
        Debug.Log(currentLevelMusic);
    }
    public void PlaySoundEffect(SoundEffect soundEffect, float soundEffectExistTime = 20f)
    {
        if (soundEffect.sound == null)
        {
            Debug.LogWarning("There is no sound in sound effect. Cant play this");
            return;
        }

        var audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.clip = soundEffect.sound;
        audioSource.pitch = Random.Range(soundEffect.minPitch, soundEffect.maxPitch);
        audioSource.volume = soundEffect.volume;
        audioSource.outputAudioMixerGroup = mixerGroupSFX;

        audioSource.Play();

        StartCoroutine(DestroyObjectAfterRealTime(audioSource.gameObject, soundEffectExistTime));
        // Destroy(audioSource.gameObject, soundEffectExistTime);
    }

    public void PlaySoundEffect(SoundEffect soundEffect, Vector2 soundEffectPosition, float soundEffectExistTime = 20f)
    {
        if (soundEffect.sound == null)
        {
            Debug.LogWarning("There is no sound in sound effect. Cant play this");
            return;
        }

        var audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.clip = soundEffect.sound;
        audioSource.pitch = Random.Range(soundEffect.minPitch, soundEffect.maxPitch);
        audioSource.volume = soundEffect.volume;
        audioSource.outputAudioMixerGroup = mixerGroupSFX;

        audioSource.Play();

        //if (soundEffectExistTime == 0)
        //{
        //    StartCoroutine(DestroyObjectAfterRealTime(audioSource.gameObject, soundEffect.sound.length));
        //}

        StartCoroutine(DestroyObjectAfterRealTime(audioSource.gameObject, soundEffectExistTime));
        // Destroy(audioSource.gameObject, soundEffectExistTime);
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
            currentLevelMusic.volume = 0;
            currentLevelMusic.Play();
            StartCoroutine(FadeInCoroutine(currentLevelMusic, 30f));
        }
    }

    IEnumerator DestroyObjectAfterRealTime(GameObject obj, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(obj);
    }


    private IEnumerator FadeInCoroutine(AudioSource music ,float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            music.volume = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }

        music.volume = 1;
    }

    private IEnumerator FadeOutCoroutine(AudioSource music,float duration)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            music.volume = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }

        music.volume = 0;

    }
}
