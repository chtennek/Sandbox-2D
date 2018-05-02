using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorBehaviour : MonoBehaviour
{
    [Header("Input")]
    public InputReceiver input;
    public string buttonName = "Interact";

    private void Reset()
    {
        input = GetComponent<InputReceiver>();
    }

    private void Awake()
    {
        if (input == null)
            Warnings.ComponentMissing(this);
    }

    private void Update()
    {
        if (input == null)
            return;
        
        if (input.GetButtonDown(buttonName))
        {
            foreach (Collider2D col in Physics2D.OverlapPointAll((Vector2)transform.position))
            {
                InteractableBehaviour target = col.GetComponent<InteractableBehaviour>();
                if (target != null)
                {
                    target.OnInteract(transform);
                }
            }
        }
    }
}
