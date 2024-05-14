using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBarier : MonoBehaviour
{
    [Header("Yellow Points")]
    [SerializeField] private YellowPoint[] yellowPoints;
    private int yellowPointsCount;

    void Start()
    {
        yellowPointsCount = yellowPoints.Length;

        foreach (var point in yellowPoints)
        {
            point.yellowBarier = this;
        }
    }

    public void DestroyOnePoint()
    {
        yellowPointsCount--;

        if (yellowPointsCount == 0)
            Destroy(gameObject);
    }
}
