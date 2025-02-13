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
    public AudioClip battleMusic;
    [SerializeField] private AudioSource currentLevelMusic;
    public AudioSource battleLevelMusic;
    [SerializeField] private AudioMixer audioMixerMusic;
    [SerializeField] private AudioMixerGroup mixerGroupSFX;

    [Header("Music Group")]
    [SerializeField] private AudioMixerGroup _normalGroup;
    public AudioMixerGroup normalGroup { get { return _normalGroup; } }

    [SerializeField] private AudioMixerGroup _battleGroup;
    public AudioMixerGroup battleGroup { get { return _battleGroup; } }

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
            StartCoroutine(TransitionToMusic(music, currentLevelMusic, 0.1f));
        }
    }
    public void SetMusic(AudioClip music, PlayerConditions playerCondition, bool restartIfMatch = false)
    {
        if (playerCondition == PlayerConditions.Battle)
        {

            if (restartIfMatch || battleLevelMusic.clip != music)
            {
                battleLevelMusic = gameObject.AddComponent<AudioSource>();
                battleLevelMusic.clip = music;
                battleLevelMusic.outputAudioMixerGroup = battleGroup;
                StartCoroutine(TransitionToMusic(music, battleLevelMusic, 0.1f));
            }
        }
    }

    IEnumerator DestroyObjectAfterRealTime(GameObject obj, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(obj);
    }



    private IEnumerator TransitionToMusic(AudioClip newMusic, AudioSource audio, float duration)
    {

        audio.clip = newMusic;
        audio.volume = 0;
        audio.Play();


        while (audio.volume < 1)
        {
            audio.volume += duration * Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator DampingToMusic(AudioSource audio, float duration)
    {

        while (audio.volume > 0)
        {
            audio.volume -= duration * Time.deltaTime;
            yield return null;
        }

        audio.clip = null;
        audio.Stop();
    }
}
