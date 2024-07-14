using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [Header("Chances")]
    [Range(0, 100)]
    [SerializeField] private int chanceOfAppearanceAnyItem = 50;
    [Range(0, 100)]
    [SerializeField] private int chanceOfAppearanceMediumItem = 20;
    [Range(0, 100)]
    [SerializeField] private int chanceOfAppearanceRareItem = 30;

    [Header("Items")]
    [SerializeField] private List<GameObject> defaultItems;
    [SerializeField] private List<GameObject> mediumItems;
    [SerializeField] private List<GameObject> rareItems;

    private void Start()
    {
        int r = Random.Range(0, 100);

        if (r < chanceOfAppearanceAnyItem)
        {
            r = Random.Range(0, 100);

            if (r < chanceOfAppearanceMediumItem)
            {
                r = Random.Range(0, 100);

                if (r < chanceOfAppearanceRareItem)
                {
                    InstantiateRandomItemFromList(rareItems);
                }
                else
                {
                    InstantiateRandomItemFromList(mediumItems);
                }
            }
            else
            {
                InstantiateRandomItemFromList(defaultItems);
            }
        }
    }

    void InstantiateRandomItemFromList(List<GameObject> list)
    {
        int i = Random.Range(0, list.Count);
        GameObject item = Instantiate(list[i]);
        item.transform.position = this.transform.position;
        item.transform.rotation = Quaternion.identity;
    }
}
