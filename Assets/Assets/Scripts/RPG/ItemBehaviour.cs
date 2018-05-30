using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sandbox.RPG;

public class ItemBehaviour : MonoBehaviour, IInteractable
{
    public ItemType type;

    public void OnInteract(Transform source)
    {
        throw new System.NotImplementedException();
    }
}
