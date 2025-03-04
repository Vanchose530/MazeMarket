using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reward After Battle Config", menuName = "Scriptable Objects/Configs/Enemyes/Reward", order = 2)]
public class RewardAfterBattleConfig : ScriptableObject
{
    [Header("Items")]
    [SerializeField] private Balancer<GameObject> items;
    [Range(0, 100)]
    [SerializeField] private int _chanceToReward = 60;
    public int chanceToReward { get { return _chanceToReward; } }

    public GameObject GetReward()
    {
        return items.Get();
    }
}
