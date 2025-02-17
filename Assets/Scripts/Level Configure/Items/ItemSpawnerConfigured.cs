using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSpawnerConfigured : MonoBehaviour
{
    public void TryToAppear()
    {
        if (ItemsConfigurator.instance.GetAppearItem())
        {
            SpawnItem();
        }

        Destroy(gameObject);
    }

    void SpawnItem()
    {
        GameObject item = Instantiate(ItemsConfigurator.instance.GetAppearItemPrefab());
        item.transform.position = this.transform.position;
        item.transform.rotation = Quaternion.identity;
    }
}
