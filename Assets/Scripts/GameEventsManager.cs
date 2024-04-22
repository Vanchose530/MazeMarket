using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    private static GameEventsManager _instance;
    public static GameEventsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<GameEventsManager>();
                _instance.name = _instance.GetType().ToString();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public InputEvents input { get; private set; }
    public PauseEvents pause { get; private set; }
    public PlayerEvents player { get; private set; }
    public PlayerWeaponsEvents playerWeapons { get; private set; }

    private void Awake()
    {
        input = new InputEvents();
        pause = new PauseEvents();
        player = new PlayerEvents();
        playerWeapons = new PlayerWeaponsEvents();
    }
}
