using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataKeeper : MonoBehaviour
{
    static PlayerDataKeeper _instance;
    public static PlayerDataKeeper instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<PlayerDataKeeper>();
                _instance.name = _instance.GetType().ToString();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    [HideInInspector] public PlayerData playerData;

    public void ClearData()
    {
        playerData = null;
    }
}
