using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public string label = "HP";
    public int minValue = 0;
    public int maxValue = 100;
    public int currentValue = 100; // Set in inspector to initial value

    [Header("Display")]
    public float lerpValue = .51f;
    [Space]
    public Text numericalDisplay;
    public int zeroPadding = 0;
    [Space]
    public Image canvasDisplay;
    public bool overrideColor = false;
    public Color[] displayColor;
    public bool useColorGradient = false;

    private void Reset()
    {
        if (numericalDisplay == null)
            numericalDisplay = GetComponent<Text>();
        if (canvasDisplay == null)
            canvasDisplay = GetComponent<Image>();
    }

    public void AddValue(int value)
    {
        currentValue = Mathf.Clamp(currentValue + value, minValue, maxValue);
    }

    public void SubtractValue(int value)
    {
        currentValue = Mathf.Clamp(currentValue - value, minValue, maxValue);
    }

    public void SetValue(int value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
    }

    public void AddPercent(float percent)
    {
        AddValue((int)Mathf.Floor(percent * maxValue));
    }

    public void SubtractPercent(float percent)
    {
        SubtractValue((int)Mathf.Floor(percent * maxValue));
    }

    public void SetPercent(float percent)
    {
        SetValue((int)Mathf.Floor(percent * maxValue));
    }

    private void Update()
    {
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);

        if (numericalDisplay != null)
        {
            int lastValue = 0;
            Int32.TryParse(numericalDisplay.text, out lastValue);
            numericalDisplay.text = Mathf.RoundToInt(Mathf.Lerp(lastValue, currentValue, lerpValue)).ToString().PadLeft(zeroPadding, '0');
        }
        if (canvasDisplay != null)
        {
            canvasDisplay.fillAmount = Mathf.Lerp(canvasDisplay.fillAmount, GetValuePercent(), lerpValue);
            if (overrideColor)
            {
                canvasDisplay.color = DetermineDisplayColor();
            }
        }
    }

    private float GetValuePercent()
    {
        return (maxValue == 0) ? 1 : (float)currentValue / maxValue;
    }

    private void SetValuePercent(float percent)
    {
        currentValue = (int)(percent * maxValue);
    }

    private Color DetermineDisplayColor()
    {
        if (displayColor.Length == 0)
            return Color.clear;
        if (displayColor.Length == 1)
            return displayColor[0];
        if (currentValue == maxValue)
            return displayColor[displayColor.Length - 1];

        // Find where we are in displayColor[]
        float percent = GetValuePercent();
        int buckets = (useColorGradient) ? displayColor.Length - 1 : displayColor.Length;
        int index = (int)Mathf.Floor(percent * buckets);
        float percentInBucket = (percent * buckets) % 1;

        if (useColorGradient)
        {
            return Color.Lerp(displayColor[index], displayColor[index + 1], percentInBucket);
        }
        else
        {
            return displayColor[index];
        }
    }
}
