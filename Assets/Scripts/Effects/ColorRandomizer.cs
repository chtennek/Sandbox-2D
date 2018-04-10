using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField] private ColorChannel colorChannel;
    [SerializeField] private float radius = 1f;

    private Renderer r;

    private void Start()
    {
        r = GetComponent<Renderer>();
        r.material.color = Randomc.ChannelRange(r.material.color, colorChannel, radius);
    }
}
