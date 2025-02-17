using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class ItemsConfigurator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ItemsConfig itemsConfig;

    public static ItemsConfigurator instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one Items Configurator in scene");
        }
        instance = this;
    }

    // true - источник лута появиться, false - не появиться
    public bool GetAppearItemSource()
    {
        return Random.Range(0, 100) <= itemsConfig.itemSourceAppearChance;
    }

    // true - лут появиться, false - не появиться
    public bool GetAppearItem()
    {
        return Random.Range(0, 100) <= itemsConfig.itemAppearChance;
    }

    public GameObject GetAppearItemPrefab()
    {
        return itemsConfig.GetItemGameObject();
    }
}
