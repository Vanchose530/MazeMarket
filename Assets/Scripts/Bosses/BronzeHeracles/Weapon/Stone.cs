using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public GameObject smallStonePrefab;
    public Transform point;
    public int damageStone;
    public int minCountSmallStone;
    public int maxCountSmallStone;
    private int randomSmallStone;
    private int[] randArr = new int[] { -1, 1 };
    [SerializeField] private float forceStone;
    private void Start()
    {
        randomSmallStone = Random.Range(minCountSmallStone, maxCountSmallStone + 1);
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(damageStone);
            Destroy(gameObject);
        }
        else {
            for (int i = 0; i < randomSmallStone; i++)
            {
                GameObject stoneSmall = Instantiate(smallStonePrefab, point.position, transform.rotation);
                Destroy(gameObject);
                Rigidbody2D srb = stoneSmall.GetComponent<Rigidbody2D>();
                srb.AddForce((-stoneSmall.transform.up * Random.value + (randArr[Random.Range(0, randArr.Length)]) * transform.right).normalized * forceStone, ForceMode2D.Impulse);
            }
        }
       
    }
    private IEnumerator Delete() 
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
