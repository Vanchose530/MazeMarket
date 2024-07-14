using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<GenerationLayer> generationLayers;

    public LevelTemplate GenerateNewLevel()
    {
        LevelTemplate resultLevel = new LevelTemplate();

        foreach (var layer in generationLayers)
        {
            if (layer.enabled)
                layer.Layer(resultLevel);
        }

        return resultLevel;
    }
}
