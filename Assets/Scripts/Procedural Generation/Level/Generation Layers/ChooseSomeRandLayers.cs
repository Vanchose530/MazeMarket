using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Some Rand Layers", menuName = "Generation Layers/Chose Some Rand Layers", order = 4)]
public class ChooseSomeRandLayers : GenerationLayer
{
    [Header("Choose Some Rand Layers")]
    [SerializeField] private int count = 1;
    [SerializeField] private Balancer<GenerationLayer> layersToChoose;

    public override void Layer(LevelTemplate levelTemplate)
    {
        for (int i = 0; i < count; i++)
        {
            var layer = layersToChoose.Get();
            layer.Layer(levelTemplate);
        }
    }
}
