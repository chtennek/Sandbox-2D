using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractorBehaviour : MonoBehaviour
{
    [Header("Properties")]
    public float sensitivity = .5f;
    public CompoundMask mask;
    public bool limitOneInteractable;

    public TransformUnityEvent onInteract;

    public void Interact()
    {
        // Check 2D and 3D for possible targets
        List<Transform> targets = new List<Transform>();
        foreach (Collider coll in Physics.OverlapSphere(transform.position, sensitivity))
            targets.Add(coll.transform);
        foreach (Collider2D coll2D in Physics2D.OverlapCircleAll(transform.position, sensitivity))
            targets.Add(coll2D.transform);

        // Filter for interactables
        foreach (Transform target in targets)
        {
            if (mask.Check(transform, target) == false)
                continue;

            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable == null)
                continue;

            interactable.OnInteract(transform);

            onInteract.Invoke(target.transform);

            if (limitOneInteractable)
                return;
        }
    }
}
