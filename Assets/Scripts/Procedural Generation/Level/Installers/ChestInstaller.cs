using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInstaller : MonoBehaviour
{
    [Header("Install Settings")]
    [SerializeField] private float border1 = 0.8f;
    [SerializeField] private float border2 = 1.2f;

    [Header("Setup")]
    [SerializeField] private GameObject defaultChest;
    [SerializeField] private GameObject mediumChest;
    [SerializeField] private GameObject rareChest;

    public void Install(float value)
    {
        if (value < border1)
        {
            // оставляем defaultChest
            Destroy(mediumChest);
            Destroy(rareChest);
        }
        else if (value >= border1 && value < border2)
        {
            Destroy(defaultChest);
            // оставляем mediumChest
            Destroy(rareChest);
        }
        else if (value >= border2)
        {
            Destroy(defaultChest);
            Destroy(mediumChest);
            // оставляем rareChest
        }
    }
}
