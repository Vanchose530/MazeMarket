using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class InteractableObjectsDetector : MonoBehaviour
{
    IInteractable _interactable;
    public IInteractable interactable
    {
        get { return _interactable; }
        private set
        {
            _interactable?.CanNotInteract(playerOwner);
            _interactable = value;
            _interactable?.CanInteract(playerOwner);
        }
    }

    [Header("Setup")]
    [SerializeField] private Player playerOwner;
    [SerializeField] private CircleCollider2D circleCollider;

    private void OnValidate()
    {
        if (playerOwner == null)
            playerOwner = GetComponentInParent<Player>();
        if (circleCollider == null)
            circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();

        if (newInteractable != null)
        {
            interactable = newInteractable;
        }

        // —ƒ≈À¿“‹, ◊“Œ¡€ Œ¡⁄≈ “€ Õ≈ Õ¿’Œƒ»À»—‹ — ¬Œ«‹ —“≈Õ”
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable outInteractable = collision.gameObject.GetComponent<IInteractable>();

        if (outInteractable == interactable)
        {
            interactable = null;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
            
            foreach (var coll in colliders)
            {
                IInteractable newInteractable = coll.gameObject.GetComponent<IInteractable>();

                if (newInteractable != null)
                {
                    interactable = newInteractable;
                    break;
                }
            }
        }
    }
}
