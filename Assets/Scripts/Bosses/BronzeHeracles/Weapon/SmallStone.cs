using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmallStone : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private int damageSmallStone;
    [SerializeField] private float timeDestroySmallStone;

    private void Start()
    {
        StartCoroutine("Delete");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();

        if (obj != null)
        {
            obj.TakeDamage(damageSmallStone, transform);
            Destroy(gameObject);
        }
    }
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(timeDestroySmallStone);
        Destroy(gameObject);
    }
}
