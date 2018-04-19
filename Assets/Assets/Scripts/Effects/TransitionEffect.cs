using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransitionEffect : MonoBehaviour
{
    public Material effect;
    public string propertyName = "_Progress";

    public float time = 1f;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private float startTime;

    public void Play()
    {
        startTime = Time.time;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (effect == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        if (Application.isPlaying)
        {
            float t = (time == 0) ? 1 : (Time.time - startTime) / time;
            float progress = curve.Evaluate(t);
            effect.SetFloat(propertyName, progress);
        }

        Graphics.Blit(source, destination, effect);
    }
}
