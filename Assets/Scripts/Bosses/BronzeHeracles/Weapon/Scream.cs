using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scream : MonoBehaviour
{
    [Header("Behavior")]
    public int damage;
    public float pushForce;
    private Vector3 initialScale;
    private Coroutine coroutine;

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamagable obj = collision.gameObject.GetComponent<IDamagable>();

        if (obj != null)
        {
            obj.TakeDamage(damage, transform);
        }
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
            playerRigidbody.AddForce(pushDirection * pushForce);
        }
    }
    public void StartScreamChange(Vector3 targetScale, float durationIn, float timeScream, float durationOut) 
    {
        coroutine = StartCoroutine(ScreamChange(targetScale, durationIn, timeScream, durationOut));    
    }

    private IEnumerator ScreamChange(Vector3 targetScale, float durationIn, float timeScream, float durationOut)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.5f);
        initialScale = new Vector3(0f, 0f, 1f);
        
        float timeChange = 0f;

        while (timeChange < durationIn)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timeChange / durationIn);
            timeChange += Time.deltaTime;
            yield return null;
        }

        
        transform.localScale = targetScale;
        transform.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f, 255f);
        yield return new WaitForSeconds(timeScream);

        transform.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.5f);
        timeChange = 0f;

        while (timeChange < durationOut)
        {
            transform.localScale = Vector3.Lerp(targetScale, initialScale, timeChange / durationOut);
            timeChange += Time.deltaTime;
            yield return null;
        }
        transform.localScale = initialScale;
        gameObject.SetActive(false);
    }
    public void StopScream() 
    {
        StopCoroutine(coroutine);
        transform.localScale = initialScale;
        gameObject.SetActive(false);
    }
}
