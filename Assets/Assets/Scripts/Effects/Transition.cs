using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Transition : MonoBehaviour {
    public Material effect;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
        if (effect == null)
            return;
        
        Graphics.Blit(source, destination, effect);
	}
}
