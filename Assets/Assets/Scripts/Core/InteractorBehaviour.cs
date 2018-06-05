using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractorBehaviour : MonoBehaviour
{
    [SerializeField]
    private string comment;

    [Header("Properties")]
    [SerializeField]
    private CompoundMask mask;

    [SerializeField]
    private float radius = .5f;

    [SerializeField]
    private bool limitOneInteractable;

    public TransformUnityEvent onInteractWith;

    public void Deallocate()
    {
        ObjectPooler.Deallocate(transform);
    }

    public void Deallocate(Transform target)
    {
        ObjectPooler.Deallocate(target);
    }

    public void InteractWith(Transform target)
    {
        if (target == null)
            return;

        IInteractable interactable = target.GetComponent<IInteractable>();
        InteractWith(target, interactable);
    }

    public bool InteractWith(Transform target, IInteractable interactable)
    {
        if (target == null || interactable == null)
            return false;

        // Call the Interactable
        interactable.OnInteractBy(transform);

        // Call our own OnInteract handling function
        onInteractWith.Invoke(target);

        return true;
    }

    public void InteractWithAll()
    {
        // Check 2D and 3D for possible targets
        List<Transform> targets = mask.GetCollidersWithin(radius, transform);

        // Filter for interactables
        foreach (Transform target in targets)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            bool success = InteractWith(target, interactable);

            if (success && limitOneInteractable)
                return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
