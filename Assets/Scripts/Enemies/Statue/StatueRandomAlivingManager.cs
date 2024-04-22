using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRandomAlivingManager : MonoBehaviour
{
    const string PATH_TO_STATUE_PREFAB = "Enemies\\Statue(Stay to Aliving)";

    [Header("General")]
    [SerializeField] private int alivingStatuesCount;
    [SerializeField] private List<GameObject> statues;

    private void Start()
    {
        try
        {
            for (int i = 0; i < alivingStatuesCount; i++)
            {
                int j = Random.Range(0, statues.Count);
                TurnIntoAliving(statues[j]);
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogWarning("Aliving statues count more than statues count. Name of game object: " + gameObject.name); 
        }
        
    }

    private void TurnIntoAliving(GameObject statue)
    {
        var alivingStatue = Instantiate(Resources.Load<GameObject>(PATH_TO_STATUE_PREFAB), statue.transform.position, statue.transform.rotation, this.transform);
        alivingStatue.transform.localScale = statue.transform.localScale;
        statues.Remove(statue);
        Destroy(statue);
    } 
}
