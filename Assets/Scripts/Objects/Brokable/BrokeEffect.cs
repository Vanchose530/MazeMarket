using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeEffect : MonoBehaviour
{
    [Header("Effect States")]
    [SerializeField] private Sprite[] effectStates;

    [Header("Setup")]
    [SerializeField] private SpriteRenderer se;

    private void OnValidate()
    {
        se = se == null ? GetComponent<SpriteRenderer>() : se;
    }

    public void SetEffectStateByRelation(float relation)
    {
        float segments = 1f / (float)effectStates.Length;

        float segmentsCount = 0;

        for (int i = 0; i < effectStates.Length; i++, segmentsCount += segments)
        {
            if (relation > segmentsCount && relation <= (segmentsCount + segments))
            {
                se.sprite = effectStates[i];

                // Debug.Log($"Relation: {relation}, State: {i}");
                return;
            }
        }

        Debug.LogWarning($"Couldn't set the breaking effect! Relation: {relation}, Segments {segments}");
    }
}
