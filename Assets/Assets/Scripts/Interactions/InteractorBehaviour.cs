using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorBehaviour : InputBehaviour
{
    [Header("Input")]
    public string inputName = "Fire";

    private void Update()
    {
        if (input.GetButtonDown(inputName))
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
