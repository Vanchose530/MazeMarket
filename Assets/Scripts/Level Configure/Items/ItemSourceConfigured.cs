using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSourceConfigured : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private ItemSpawnerConfigured[] itemSpawners;

    [ContextMenu("Check Item Spawners in children")]
    private void CheckItemSpawnersInChildren()
    {
        itemSpawners = GetComponentsInChildren<ItemSpawnerConfigured>();
    }

    private void Start()
    {
        if (ItemsConfigurator.instance == null)
        {
            Debug.LogError("There isn't Items Configurator in scene to configure items");
            Destroy(gameObject);
            return;
        }

        TryToAppear();
    }

    private void TryToAppear()
    {
        if (ItemsConfigurator.instance.GetAppearItemSource())
        {
            foreach (var spawner in itemSpawners)
            {
                spawner.TryToAppear();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
