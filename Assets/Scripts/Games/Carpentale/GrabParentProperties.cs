using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabParentProperties : MonoBehaviour
{
    private Transform parent;
    private Renderer parentRenderer;

    private Renderer renderer;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        UpdateFromParent();
    }

    void Update()
    {
        if (parent != transform.parent)
            UpdateFromParent();
        else if (parentRenderer != null && parentRenderer.material.color != renderer.material.color)
            renderer.material.color = parentRenderer.material.color;
    }

    public void UpdateFromParent()
    {
        parent = transform.parent;
        parentRenderer = parent.GetComponent<Renderer>();

        if (parentRenderer == null)
            renderer.material.color = Color.clear;
        else
            renderer.material.color = parentRenderer.material.color;
    }
}
