using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAfterBattleConfigured : MonoBehaviour
{
    public void SpawnReward()
    {

        if (EnemyesConfigurator.instance.GetAppearReward())
        {
            GameObject reward = EnemyesConfigurator.instance.GetReward();

            var inGame = Instantiate(reward);

            inGame.transform.position = transform.position;
        }
        

        Destroy(this.gameObject);
    }
}
