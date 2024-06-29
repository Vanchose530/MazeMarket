using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public float bonusValue { get; set; }
    public BonusType bonusType { get; set; }
    public EnemyesOnRoom enemyesOnRoom { get; set; }
    public RoomLockType lockType { get; set; }
}
