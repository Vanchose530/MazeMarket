using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour, IDataPersistence
{
    public static ItemsManager instance { get; private set; }

    [HideInInspector] public List<string> collectedItemsId;


    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Items Manager in scene");
        instance = this;
    }



    public void LoadData(GameData data)
    {
        this.collectedItemsId = data.collectedItemsId;
    }

    public void SaveData(ref GameData data)
    {
        data.collectedItemsId = this.collectedItemsId;
    }
}
