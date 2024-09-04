using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScriptDeactivator : MonoBehaviour
{
    [Header("Deactivator")]
    [SerializeField] private List<MonoBehaviour> scripts;
    [Range(0, 100)]
    [SerializeField] private int chance;

    private void Start()
    {
        if (chance == 0)
            Destroy(this);
        
        int r = Random.Range(0, 100);

        if (r <= chance)
        {
            foreach (var script in scripts)
            {
                Destroy(script);
            }
        }

        Destroy(this);
    }
}
