using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBehaviour : MonoBehaviour {
    public abstract void OnInteract(Transform source);
}
