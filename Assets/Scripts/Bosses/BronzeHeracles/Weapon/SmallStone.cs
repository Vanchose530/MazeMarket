using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmallStone : MonoBehaviour
{
    [SerializeField]private int damageSmallStone;

    private void Start()
    {
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(damageSmallStone);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Walls")
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
