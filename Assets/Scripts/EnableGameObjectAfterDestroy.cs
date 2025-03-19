using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectAfterDestroy : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] private GameObject go;

    private void Start()
    {
        go.SetActive(false);
    }

    private void OnDestroy()
    {
        go.SetActive(true);
    }
}
