using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScoreCounter : MonoBehaviour
{
    static SimpleScoreCounter _instance;
    public static SimpleScoreCounter instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<SimpleScoreCounter>();
                _instance.name = _instance.GetType().ToString();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public int defeatedEnemyesCount { get; private set; }

    public float levelTime { get; private set; }

    public bool countLevelTime;

    private void Awake()
    {
        countLevelTime = false;
        levelTime = 0f;
    }

    private void Update()
    {
        if (countLevelTime)
        {
            levelTime += Time.deltaTime;
        }
    }

    public void AddDefeatedEnemy()
    {
        defeatedEnemyesCount++;
    }

    public void Reset()
    {
        defeatedEnemyesCount = 0;
        levelTime = 0;
        countLevelTime = false;
    }
}
