using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAmmo : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private List<GameObject> list;

    private void Start()
    {
        int r = Random.Range(0, list.Count);
        GameObject item = Instantiate(list[r]);
        item.transform.position = this.transform.position;
        item.transform.rotation = Quaternion.identity;
    }
}
