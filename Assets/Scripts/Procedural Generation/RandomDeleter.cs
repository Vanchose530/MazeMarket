using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeleter : MonoBehaviour
{
    [Header("Deleter")]
    [Range(0, 100)]
    [SerializeField] private int chance = 50;

    private void Start()
    {
        if (chance == 0)
            Destroy(this);

        int r = Random.Range(0, 100);

        // Debug.Log(gameObject.name + " delete chance: " + chance + ", r: " + r + ", delete: " + (r <= chance));

        if (r <= chance)
        {
            Destroy(this.gameObject);
        }

        Destroy(this);
    }
}
