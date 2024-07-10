using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choose Rooms With Enemy Waves Layer", menuName = "Generation Layers/Chose Rooms With Enemy Waves", order = 8)]
public class ChooseRoomsWithEnemyWavesLayer : GenerationLayer
{
    [Header("Max Transisted Rooms Bonus Values Sum to Enemy Wave")]
    public float oneWaveMTRBVS = 0.4f;
    public float twoWavesMTRBVS = 1.5f;

    public override void Layer(LevelTemplate levelTemplate)
    {
        List<Vector2Int> roomPositions = levelTemplate.levelRoomsPositions;

        foreach (Vector2Int roomPosition in roomPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPosition.x, roomPosition.y];

            double transRoomsBonusValueSum = 0;

            HashSet<Vector2Int> transRoomPoss = room.GetTransistedRoomsPositions();

            foreach (var transRoomPos in transRoomPoss)
            {
                RoomTemplate transRoom = levelTemplate.levelRooms[transRoomPos.x, transRoomPos.y];

                transRoomsBonusValueSum += transRoom.bonusValue;
            }

            transRoomsBonusValueSum -= room.bonusValue;

            if (transRoomsBonusValueSum <= 0)
            {
                room.enemyesOnRoom = EnemyesOnRoom.None;
            }
            else if (transRoomsBonusValueSum <= oneWaveMTRBVS)
            {
                room.enemyesOnRoom = EnemyesOnRoom.OneWave;
            }
            else if (transRoomsBonusValueSum <= twoWavesMTRBVS)
            {
                room.enemyesOnRoom = EnemyesOnRoom.TwoWaves;
            }
            else if (transRoomsBonusValueSum > twoWavesMTRBVS)
            {
                room.enemyesOnRoom = EnemyesOnRoom.ThreeWaves;
            }
        }

        foreach (Vector2Int roomPosition in roomPositions)
        {
            RoomTemplate room = levelTemplate.levelRooms[roomPosition.x, roomPosition.y];

            if (room.enemyesOnRoom != EnemyesOnRoom.None && room.bonusValue > 0)
            {
                switch (room.enemyesOnRoom)
                {
                    case EnemyesOnRoom.OneWave:
                        room.bonusValue += 0.1f;
                        break;
                    case EnemyesOnRoom.TwoWaves:
                        room.bonusValue += 0.15f;
                        break;
                    case EnemyesOnRoom.ThreeWaves:
                        room.bonusValue += 0.2f;
                        break;
                }
            }
        }
    }
}
