using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsStorage : MonoBehaviour
{
    private static EffectsStorage _instance;

    public static EffectsStorage instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<GameObject>(PATH_TO_SINGLETON_PREFAB);
                var inScene = Instantiate<GameObject>(prefab);
                _instance = inScene.GetComponentInChildren<EffectsStorage>();

                if (_instance == null)
                    _instance = inScene.AddComponent<EffectsStorage>();
            }
            return _instance;
        }
    }

    const string PATH_TO_SINGLETON_PREFAB = "Storages\\Spawn Effects Storage";

    [Header("Effects")]
    public GameObject enemySpawnEffect;

    [Header("Sounds")]
    public GameObject enemySpawnEffectSound;
    public GameObject enemySpawnSound;
}
