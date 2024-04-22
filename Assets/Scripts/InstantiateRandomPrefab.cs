using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRandomPrefab : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    GameObject pref;

    private void Start()
    {
        pref = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(pref);
    }

    private void OnDestroy()
    {
        Destroy(pref);
    }
}
