
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

    public void CreateMoney(int count)
    {
        if (count < 1) return;
        
        string path = PATH_TO_ITEMS + "Money\\";
        
        Instantiate<GameObject>(Resources.Load<GameObject>(path));//определять сколько и какого номинала спавнить
    }


}
