using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    static EffectsManager _instance;
    public static EffectsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<EffectsManager>();
                _instance.name = _instance.GetType().ToString();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void PlayEffect(GameObject effectPrefab, Transform effectTransform, float effectExistTime = 20f)
    {
        var effect = Instantiate(effectPrefab, effectTransform);
        Destroy(effect, effectExistTime);
    }

    public void PlayEffect(GameObject effectPrefab, Vector3 effectPosition, Quaternion effectRotation, float effectExistTime = 20f)
    {
        var effect = Instantiate(effectPrefab, effectPosition, effectRotation);
        Destroy(effect, effectExistTime);
    }

    public void PlaySoundEffect(GameObject soundEffectPrefab, float soundEffectExistTime = 20f, float minPitch = 1f, float maxPitch = 1f)
    {
        var effect = Instantiate(soundEffectPrefab);
        effect.GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
        Destroy(effect, soundEffectExistTime);
    }

    public void PlaySoundEffect(GameObject soundEffectPrefab, Vector2 soundEffectPosition, float soundEffectExistTime = 20f, float minPitch = 1f, float maxPitch = 1f)
    {
        var effect = Instantiate(soundEffectPrefab, soundEffectPosition, Quaternion.identity);
        effect.GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
        Destroy(effect, soundEffectExistTime);
    }
}
