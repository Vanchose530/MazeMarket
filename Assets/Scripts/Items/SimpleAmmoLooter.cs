using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAmmoLooter : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private AmmoItem ammoItemPrefab;

    [Header("Loot Settings")]
    [SerializeField] private int minCount;
    [SerializeField] private int maxCount;

    [Header("Drop Force")]
    [SerializeField] private float itemDropForce = 0.2f;
    [SerializeField] private float itemDropInnacuracy = 0.1f;

    private void OnDestroy()
    {
        SpawnAmmoItems();
    }

    void SpawnAmmoItems()
    {
        int count = Random.Range(minCount, maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            AmmoItem item = Instantiate(ammoItemPrefab);
            item.transform.position = this.transform.position;

            Vector2 vec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            float force = Random.Range(itemDropForce - itemDropInnacuracy, itemDropForce + itemDropInnacuracy);
            item.rb.AddForce(vec * force, ForceMode2D.Impulse);
        }
    }
}
