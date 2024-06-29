using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Layer Shuffler", menuName = "Generation Layers/Shufflers/Base", order = 10)]
public class LayersShuffler : GenerationLayer
{
    [Header("Layers to shuffle")]
    [SerializeField] private List<GenerationLayer> layersToShuffle;

    public override void Layer(LevelTemplate levelTemplate)
    {
        List<GenerationLayer> shuffledLayers = GetShuffledLayers();

        foreach (var layer in shuffledLayers)
        {
            layer.Layer(levelTemplate);
        }
    }

    private List<GenerationLayer> GetShuffledLayers()
    {
        System.Random random = new System.Random();

        GenerationLayer[] arr = layersToShuffle.OrderBy(x => random.Next()).ToArray();

        List<GenerationLayer> res = new List<GenerationLayer>();

        foreach (GenerationLayer layer in arr)
        {
            res.Add(layer);
        }

        return res;
    }
}
