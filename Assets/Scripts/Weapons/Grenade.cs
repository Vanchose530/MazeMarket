using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class Grenade : MonoBehaviour
{
    [Header("Setting Explotion")]
    [SerializeField] private float explotionRadius;
    [SerializeField] private float forceGrenadeThrow;
    [SerializeField] private LayerMask mintMiasmaLayerNumber;

    [Header("Time")]
    [SerializeField] private float timeToExplotion = 2f;

    [Header("Damage")]
    [SerializeField] private int explotionDamage = 10;
    [SerializeField] private bool damagePlayer = false;

    [Header("Effects")]
    [SerializeField] private GameObject explotionEffectPrefab;
    [SerializeField] private SoundEffect explotionSE;

    [Header("Setup")]
    [SerializeField] Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("ExplosionCoroutine");
    }

    void Explosion2D()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explotionRadius);

        if (explotionEffectPrefab != null)
        {
            var effect = Instantiate(explotionEffectPrefab, transform.position, transform.rotation);
            effect.transform.localScale = effect.transform.localScale * explotionRadius;
            Destroy(effect, 0.25f);
        }
        
        if (explotionSE != null)
        {
            AudioManager.instance.PlaySoundEffect(explotionSE, transform.position);
        }

        foreach (Collider2D hit in colliders)
        {
            if (!damagePlayer && hit.gameObject.tag == "Player")
            {
                continue;
            }

            if ((mintMiasmaLayerNumber & (1 << hit.gameObject.layer)) != 0) // сложная система проверки слоя на наличие в маски слоёв 
            {
                Destroy(hit.gameObject);
                continue;
            }

            Vector2 dir = hit.transform.position - transform.position;

            Rigidbody2D hitRigidBody = hit.gameObject.GetComponent<Rigidbody2D>();
            if (hitRigidBody != null)
            {
                hitRigidBody.AddForce(dir * forceGrenadeThrow);
            }
            //hit.gameObject.GetComponent<Rigidbody2D>()?.AddForce(dir * forceGrenadeThrow);

            hit.gameObject.GetComponent<IDamagable>()?.TakeDamage(explotionDamage, transform);
        }
    }
    IEnumerator ExplosionCoroutine()
    {
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
