using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Bonus Layer Shuffler", menuName = "Generation Layers/Shufflers/Choose Bonus", order = 13)]
public class ChooseBonusLayerShuffler : GenerationLayer
{
    [Header("Layers to shuffle")]
    [SerializeField] private List<GenerationLayer> layersToShuffle;

    [Header("Another")]
    [SerializeField] private bool checkForOneChest = false;

    public override void Layer(LevelTemplate levelTemplate)
    {
        List<GenerationLayer> shuffledLayers = GetShuffledLayers();

        foreach (var layer in shuffledLayers)
        {
            layer.Layer(levelTemplate);

            if (checkForOneChest)
            {
                if (levelTemplate.GetBonusRoomsPositions().Count == 1)
                {
                    return;
                }
            }
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
