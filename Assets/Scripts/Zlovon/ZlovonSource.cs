using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ZlovonSource : MonoBehaviour
{
    [Header("Setup and Settings")]
    [SerializeField]private GameObject zlovon;
    [SerializeField]private float spawnZlovon = 0.3f;


    void Start()
    {
        StartCoroutine("CreateZlovon");
    }

    private IEnumerator CreateZlovon() {

        yield return new WaitForSeconds(2.5f);

        while (true)
        {
            Instantiate(zlovon, transform.position, transform.rotation);

            yield return new WaitForSeconds(spawnZlovon);
        }
    }
    
    
}
