using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Garant Min Chest Rarity Layer", menuName = "Generation Layers/Garant Min Chest Rarity", order = 10)]
public class GarantMinChestRarityLayer : GenerationLayer
{
    [Header("Min Chest Rarity")]
    [SerializeField] private float minChestRarity = 2f;

    public override void Layer(LevelTemplate levelTemplate)
    {
        float realMinChestRarity = 0;

        foreach (var roomPos in levelTemplate.levelRoomsPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

            if (room.bonusType == BonusType.Chest && room.bonusValue > realMinChestRarity)
                realMinChestRarity = room.bonusValue;
        }

        if (realMinChestRarity == 0)
        {
            Debug.Log("There is no chests in level to check rarity!");
            return;
        }

        if (realMinChestRarity < minChestRarity)
        {
            Debug.Log("There is no chests with rarity more than Min Chest Rarity value. Layer will make them");

            RoomTemplate maxRarityRoom = null;

            foreach (var roomPos in levelTemplate.levelRoomsPositions)
            {
                RoomTemplate room = levelTemplate.levelRooms[roomPos.x, roomPos.y];

                if (room.bonusType == BonusType.Chest)
                {
                    if (maxRarityRoom == null)
                        maxRarityRoom = room;
                    else if (room.bonusValue > maxRarityRoom.bonusValue)
                        maxRarityRoom = room;
                }
            }

            maxRarityRoom.bonusValue = minChestRarity;
        }
    }
}
