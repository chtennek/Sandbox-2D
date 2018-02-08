using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public MonoBehaviour[] screenScripts;
    public float shakeTime = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // For debugging/tweaking
        {
            Shake();
        }
    }

    public void Shake()
    {
        transform.DOShakePosition(shakeTime);
    }
}
