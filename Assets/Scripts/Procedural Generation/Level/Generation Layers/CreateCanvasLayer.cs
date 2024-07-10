using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create Canvas Layer", menuName = "Generation Layers/Create Canvas", order = 1)]
public class CreateCanvasLayer : GenerationLayer
{
    [Header("Canvas Size")]
    public int xCanvasSize = 11;
    public int yCanvasSize = 11;

    public override void Layer(LevelTemplate levelTemplate)
    {
        levelTemplate.levelRooms = new RoomTemplate[xCanvasSize, yCanvasSize];
    }
}
