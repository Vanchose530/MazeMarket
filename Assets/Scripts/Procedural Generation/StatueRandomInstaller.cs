using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRandomInstaller : MonoBehaviour
{
    [Header("Aliving Chance")]
    [Range(0, 100)]
    [SerializeField] private int alivingChance = 50;

    [Header("Setup")]
    [SerializeField] private GameObject notAlivingStatue;
    [SerializeField] private GameObject alivingStatuePrefab;

    private void Start()
    {
        int r = Random.Range(0, 100);

        if (r <= alivingChance)
        {
            var statue = Instantiate(alivingStatuePrefab);

            statue.transform.position = notAlivingStatue.transform.position;
            statue.transform.rotation = notAlivingStatue.transform.rotation;
            statue.transform.localScale = notAlivingStatue.transform.localScale;

            Destroy(notAlivingStatue);
        }

        Destroy(this);
    }
}
