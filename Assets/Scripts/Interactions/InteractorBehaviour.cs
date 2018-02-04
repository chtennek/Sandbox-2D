using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorBehaviour : MonoBehaviour
{
    public string actionId = "Fire";

    private InputReceiver input;

    private void Awake()
    {
        input = transform.root.GetComponentInChildren<InputReceiver>();
        if (input == null)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (input.GetButtonDown(actionId))
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
