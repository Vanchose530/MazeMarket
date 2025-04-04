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
            if (_interactable != null)
                GameEventsManager.instance.interactableObjects.CanNotInteractObject();
            _interactable?.CanNotInteract(playerOwner);

            _interactable = value;

            if (_interactable != null)
                GameEventsManager.instance.interactableObjects.CanInteractObject();
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

    private void OnEnable()
    {
        GameEventsManager.instance.interactableObjects.onCanInteractObject += HintsManager.instance.ShowInteractHint;
        GameEventsManager.instance.interactableObjects.onCanNotInteractObject += HintsManager.instance.HideInteractHint;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.interactableObjects.onCanInteractObject -= HintsManager.instance.ShowInteractHint;
        GameEventsManager.instance.interactableObjects.onCanNotInteractObject -= HintsManager.instance.HideInteractHint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();

        if (newInteractable != null)
        {
            interactable = newInteractable;
        }

        // �������, ����� ������� �� ���������� ������ �����
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

    public void StartRemoveFromInteracteble(IInteractable outInteractable)
    {
        StartCoroutine(RemoveFromInteracteble(outInteractable));
    }

    private IEnumerator RemoveFromInteracteble(IInteractable outInteractable)
    {
        yield return new WaitForSeconds(0.1f);

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
