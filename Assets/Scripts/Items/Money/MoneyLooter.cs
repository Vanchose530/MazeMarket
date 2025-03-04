using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class MoneyLooter : MonoBehaviour
{
    [Header("Money Items")]
    [SerializeField] private List<MoneyItem> moneyItems;
    List<int> nominals;

    [Header("Loot Settings")]
    [SerializeField] private int count;
    [SerializeField] private int countInnacuracy;
    [SerializeField] private float splitting—oefficient = 1f;

    [Header("Drop Force")]
    [SerializeField] private float moneyDropForce;
    [SerializeField] private float moneyDropInnacuracy;
    
    private void Start()
    {
        nominals = new List<int>();
        foreach (MoneyItem money in moneyItems)
        {
            nominals.Add(money.count);
        }
        nominals.Sort();
    }

    private void OnDestroy()
    {
        SpawnMoneyItems();
    }

    MoneyItem GetMoneyItemByCount(int count)
    {
        foreach (var item in moneyItems)
        {
            if (item.count == count)
                return item;
        }
        return null;
    }

    public void SpawnMoneyItems()
    {
        int rCount = Random.Range(count - countInnacuracy, count + countInnacuracy);
        List<int> moneyNominals = GenerateMoneyVariant(rCount);

        foreach (var nom in moneyNominals)
        {
            MoneyItem item = Instantiate(GetMoneyItemByCount(nom));
            item.transform.position = this.transform.position;

            Vector2 vec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            float force = Random.Range(moneyDropForce - moneyDropInnacuracy, moneyDropForce + moneyDropInnacuracy);
            item.rb.AddForce(vec * force, ForceMode2D.Impulse);
        }

        Destroy(this.gameObject);
    }

    List<int> GenerateMoneyVariant(int sum, int outerSum = 0)
    {
        int cur = 0;
        List<int> res = new List<int>();

        if (outerSum == 0)
            outerSum = sum;

        for (int i = nominals.Count - 1; i >= 0;)
        {
            if (cur + nominals[i] < sum)
            {
                cur += nominals[i];
                res.Add(nominals[i]);
            }
            else
            {
                i--;
            }
        }

        int rc = res.Count - 1;

        List<int> addList = new List<int>();
        List<int> removeList = new List<int>();

        for (int i = 0; i < rc; i++)
        {
            if (res[i] > nominals[0])
            {
                int r = Random.Range(0, 100);

                float splitChance = ((float)res[i] / (float)outerSum) * splitting—oefficient * 100;

                //Debug.Log(splitChance);
                //Debug.Log(r);
                if (r <= splitChance)
                {
                    // Debug.Log("Split");
                    List<int> added = GenerateMoneyVariant(res[i], outerSum);

                    removeList.Add(res[i]);
                    //res.Remove(res[i]);

                    foreach (var add in added)
                    {
                        addList.Add(add);
                        // res.Add(add);
                    }
                }
            }
        }

        foreach (var add in addList)
        {
            res.Add(add);
        }

        foreach (var rem in removeList)
        {
            res.Remove(rem);
        }

        return res;
    }
}
