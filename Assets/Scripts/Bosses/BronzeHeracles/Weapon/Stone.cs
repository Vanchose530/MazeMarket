using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [Header("Behavior")]
    public GameObject smallStonePrefab;
    public Transform point;
    public int damageStone;
    public int minCountSmallStone;
    public int maxCountSmallStone;
    private int randomSmallStone;
    private int[] randArr = new int[] { -1, 1 };
    [SerializeField] private float forceStone;
    [Header("Sound")]
    [SerializeField] private SoundEffect hitSE;
    private void Start()
    {
        randomSmallStone = Random.Range(minCountSmallStone, maxCountSmallStone + 1);
        StartCoroutine("Delete");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();
        
        if (obj != null) 
        {
            obj.TakeDamage(damageStone, transform);
            Destroy(gameObject);
        }
        else {
            for (int i = 0; i < randomSmallStone; i++)
            {
                GameObject stoneSmall = Instantiate(smallStonePrefab, point.position, transform.rotation);
                Rigidbody2D srb = stoneSmall.GetComponent<Rigidbody2D>();
                srb.AddForce((-stoneSmall.transform.up * Random.value + (randArr[Random.Range(0, randArr.Length)]) * transform.right).normalized * forceStone, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }


        AudioManager.instance.PlaySoundEffect(hitSE,transform.position);
    }
    private IEnumerator Delete() 
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
