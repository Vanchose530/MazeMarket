using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEqualizer : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.rotation = Quaternion.identity;
        Destroy(this);
    }
}
