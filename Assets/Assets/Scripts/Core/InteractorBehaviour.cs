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
    private Vector3 offset = Vector3.zero;

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

    public void Interact()
    {
        // Check 2D and 3D for possible targets
        List<Transform> targets = new List<Transform>();

        // 3D
        foreach (Collider coll in Physics.OverlapSphere(transform.position + offset, radius)) {
            if (coll.attachedRigidbody == null)
                targets.Add(coll.transform);
            else
                targets.Add(coll.attachedRigidbody.transform);
        }

        // 2D
        foreach (Collider2D coll2D in Physics2D.OverlapCircleAll(transform.position + offset, radius)) {
            if (coll2D.attachedRigidbody == null)
                targets.Add(coll2D.transform);
            else
                targets.Add(coll2D.attachedRigidbody.transform);
        }

        // Filter for interactables
        foreach (Transform target in targets)
        {
            if (mask.Check(transform, target) == false)
                continue;

            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable == null)
                continue;

            // Call the Interactable
            interactable.OnInteractBy(transform);

            // Call our own OnInteract handling function
            onInteractWith.Invoke(target.transform);

            if (limitOneInteractable)
                return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
