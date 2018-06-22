using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EchoGridEntity : GridEntity
{
    [Header("Properties")]
    public Vector3Int inputScaleFactor = Vector3Int.one;

    public Color group;

    [Header("References")]
    [SerializeField]
    private Material material;
    private Renderer[] renderers;

    protected override void Awake()
    {
        base.Awake();
        renderers = GetComponentsInChildren<Renderer>();
        SetColor(group);
    }

    public void SetColor(Color color)
    {
        foreach (Renderer r in renderers)
        {
            if (material == r.material)
                r.material.color = color;
        }
    }
}
