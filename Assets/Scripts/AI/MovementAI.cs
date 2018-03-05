using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIInputReceiver))]
public class MovementAI : MonoBehaviour
{
    protected AIInputReceiver input;
    protected Collider2D coll2D;

    protected virtual void Awake() {
        input = GetComponent<AIInputReceiver>();
        coll2D = GetComponent<Collider2D>();
    }
}
