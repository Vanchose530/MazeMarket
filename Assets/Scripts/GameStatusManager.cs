using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    private static GameStatusManager _instance;
    public static GameStatusManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<GameStatusManager>();
                _instance.name = _instance.GetType().ToString();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
}
