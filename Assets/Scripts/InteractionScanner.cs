using UnityEngine;
using System;

public class InteractionScanner : MonoBehaviour
{
    public float scanRadius = 2.5f;

    public event Action<IInteractable> OnInteractableInRange;
    public event Action OnInteractableOutOfRange;

    private IInteractable currentInteractable;

    private void Update()
    {
        IInteractable closest = FindClosestInteractable();

        if (closest != currentInteractable)
        {
            currentInteractable = closest;

            if (currentInteractable != null)
            {
                OnInteractableInRange?.Invoke(currentInteractable);
            }
            else
            {
                OnInteractableOutOfRange?.Invoke();
            }
        }
    }

    public void InteractWithCurrentObject()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact(gameObject);   
            currentInteractable = null;
            OnInteractableOutOfRange?.Invoke();
        }
    }

    private IInteractable FindClosestInteractable()
    {
        IInteractable closest = null;
        float minDistance = scanRadius;

        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRadius);
        foreach (Collider col in colliders)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }

        return closest;
    }
}
