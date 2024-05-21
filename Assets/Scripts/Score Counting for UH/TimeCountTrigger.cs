using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCountTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SimpleScoreCounter.instance.countLevelTime = true;
            Destroy(this.gameObject);
        }
    }
}
