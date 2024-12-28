using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBalancerItem : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private Balancer<GameObject> items;

    [Header("Settings")]
    // Время, чтобы заспавнить предмет необходимо для правильной работы скрипта-удалятора
    // Если скрипта-удалятора нет на этом объекте, то этот параметр можно сделать равным 0
    [SerializeField] private float timeToSpawnItem = 0.1f;

    Coroutine spawnItemCoroutine;
    
    void Start()
    {
        if (timeToSpawnItem <= 0)
        {
            SpawnItem();
        }
        else
        {
            spawnItemCoroutine = StartCoroutine(SpawnItemCoroutine(timeToSpawnItem));
        }
    }

    void SpawnItem()
    {
        GameObject item = Instantiate(items.Get());
        item.transform.position = this.transform.position;
        item.transform.rotation = Quaternion.identity;
    }

    IEnumerator SpawnItemCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnItem();
    }

    private void OnDestroy()
    {
        StopCoroutine(spawnItemCoroutine);
    }
}
