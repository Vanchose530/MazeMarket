using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector2 startPosition;
    public string levelName;

    public int playerHealth;

    public int lightBulletsCount;
    public int mediumBulletsCount;
    public int heavyBulletsCount;
    public int shellsCount;

    public int keyCardCount;

    public SerializableDictionary<string, int> playerWeapons; // string - weapon name, int - ammo in magazine

    public List<string> collectedItemsId;

    public List<string> unlockedDoorsId;

    public List<string> passedRoomsId;

    public GameData()
    {
        playerHealth = 10000;
        lightBulletsCount = 200;
        mediumBulletsCount = 120;
        heavyBulletsCount = 50;
        shellsCount = 40;
        keyCardCount = 0;
        playerWeapons = new SerializableDictionary<string, int>();
        collectedItemsId = new List<string>();
        unlockedDoorsId = new List<string>();
        passedRoomsId = new List<string>();
    }
}
