
using UnityEngine;

public class CreateAssetsManager : MonoBehaviour
{
    public static CreateAssetsManager instance { get; private set; }

    private static string PATH_TO_ITEMS = "Items\\";

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Find more than one Economy Manager script in scene");
        instance = this;
    }

    public void CreateMoney(int count, Vector3 position, Quaternion rotation)
    {
        if (count < 1) return;

        int paperCount = count / 5;
        if(paperCount >= 1)
        {
            Debug.Log("paper count " + paperCount);
            count -= (5 * paperCount);
        }
        int goldCount = count / 3;
        if (goldCount >= 1)
        {
            Debug.Log ("gold " + goldCount);
            count -= (3 * paperCount);
        }
        Debug.Log("Count " + count);
        string path = PATH_TO_ITEMS + "Coins\\";
        
        for(int i = 0; i < paperCount; i++) 
        { 
            Instantiate<GameObject>(Resources.Load<GameObject>(path + "Paper Coin"), position, rotation);
        }
        for (int i = 0; i < goldCount; i++)
        {
            Instantiate<GameObject>(Resources.Load<GameObject>(path + "Gold Coin"), position, rotation);
        }
        for (int i = 0; i < count; i++)
        {
            Instantiate<GameObject>(Resources.Load<GameObject>(path + "Silver Coin"), position, rotation);
        }
    }


}
