using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create First Room Layer", menuName = "Generation Layers/Create First Room", order = 2)]
public class CreateFirstRoomLayer : GenerationLayer
{
    [Header("First room settings")]
    public int xPosition = 5;
    public int yPosition = 5;
    [Range(1f, 4f)]
    public int transitionsCount;


    public override void Layer(LevelTemplate levelTemplate)
    {
        RoomTemplate startRoom = levelTemplate.CreateRoomAtPosition(xPosition, yPosition);
        startRoom.maxTransitionsCount = 1;
        startRoom.canHaveBonus = false;
        levelTemplate.startRoom = startRoom;
    }
}
