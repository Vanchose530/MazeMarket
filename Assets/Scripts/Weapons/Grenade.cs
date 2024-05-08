using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [Header("Rigidbody")]
    [SerializeField] Rigidbody2D rb;

    [Header("Setting Explotion")]
    [SerializeField] private float explotionRadius;
    [SerializeField] private float forceGrenadeThrow;
    [SerializeField] private LayerMask layerMask;

    [Header("Time")]
    [SerializeField] private float timeToExplotion = 2f;

    [Header("Damage")]
    [SerializeField] private int explotionDamage = 10;

    [Header("Prefab")]
    [SerializeField] private GameObject explotionEffectPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("ExplosionCoroutin");
    }

    
    
    void Explosion2D()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explotionRadius, layerMask);
        
        foreach (Collider2D hit in colliders)
        {
            if (hit.gameObject.layer==17) {
                Destroy(hit.gameObject);
                continue;
            }
            Vector2 dir = hit.transform.position - transform.position;
            hit.GetComponent<Rigidbody2D>().AddForce(dir * forceGrenadeThrow);
            hit.gameObject.GetComponent<IDamagable>().TakeDamage(explotionDamage);
        }
    }
    IEnumerator ExplosionCoroutin() {
        yield return new WaitForSeconds(timeToExplotion);
        Explosion2D();
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explotionRadius);
    }
}
