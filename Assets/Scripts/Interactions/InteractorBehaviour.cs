using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorBehaviour : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputReceiver input;
    public string inputName = "Fire";

    private void Awake()
    {
        if (input == null)
        {
            Debug.LogWarning(Warnings.RequireComponent(this));
            enabled = false;
        }
    }

    private void Update()
    {
        if (input.GetButtonDown(inputName))
        {
            foreach (Collider2D col in Physics2D.OverlapPointAll((Vector2)transform.position))
            {
                InteractableBehaviour target = col.GetComponent<InteractableBehaviour>();
                if (target != null)
                {
                    Debug.Log(target);
                    target.OnInteract(transform);
                }
            }
        }
    }
}
