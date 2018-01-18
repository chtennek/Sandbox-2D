using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(EntitySelector))]
public class OrderFreeMove : MonoBehaviour
{
    private Mouse mouse;
    private EntitySelector es;

    private void Awake()
    {
        mouse = ReInput.controllers.Mouse;
        es = GetComponent<EntitySelector>();
    }

    private void Update()
    {
        if (mouse.GetButtonDown(1))
        {
            Vector2 destination = Camera.main.ScreenToWorldPoint(mouse.screenPosition);
            foreach (EntityManager entity in es.GetSelectedObjects())
            {
                entity.QueueMoveOrder(destination);
            }
        }
    }
}
