using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sandbox.RPG;

public class ItemBehaviour : MonoBehaviour, IInteractable
{
    public ItemType type;

    [SerializeField]
    private TransformUnityEvent onInteractBy;

    public void OnInteractBy(Transform source)
    {
        onInteractBy.Invoke(source);
    }
}
