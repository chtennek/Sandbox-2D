using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InGameMenuMessenger : MonoBehaviour, ICancelHandler
{
    public ICancelHandler target;

    private void Awake()
    {
        if (target == null && transform.parent != null)
            target = transform.parent.GetComponentInParent<ICancelHandler>();
    }

    public void OnCancel(BaseEventData eventData)
    {
        target.OnCancel(eventData);
    }
}
