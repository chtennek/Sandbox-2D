using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameValue : MonoBehaviour
{
    public string label = "HP";
    public float minValue = 0;
    public float maxValue = Mathf.Infinity;
    public float m_currentValue = 0; // Initial value
    public bool roundToInt = true;
    public float Value
    {
        get { return m_currentValue; }
        set
        {
            m_currentValue = Mathf.Clamp(value, minValue, maxValue);
            if (roundToInt == true)
                m_currentValue = Mathf.Round(m_currentValue);
        }
    }

    [Header("Display")]
    public float m_lerpValue = .51f;
    [Space]

    public Text m_numericalDisplay;
    public int m_zeroPadding = 0;

    [Space]

    public Image m_canvasDisplay;
    public bool m_overrideColor = false;
    public Color[] m_displayColor;
    public bool m_useColorGradient = false;

    private void Reset()
    {
        if (m_numericalDisplay == null)
            m_numericalDisplay = GetComponent<Text>();
        if (m_canvasDisplay == null)
            m_canvasDisplay = GetComponent<Image>();
    }

    public void AddValue(int value)
    {
        Value = Mathf.Clamp(Value + value, minValue, maxValue);
    }

    public void SubtractValue(int value)
    {
        Value = Mathf.Clamp(Value - value, minValue, maxValue);
    }

    public void SetValue(int value)
    {
        Value = Mathf.Clamp(value, minValue, maxValue);
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
        Value = Mathf.Clamp(Value, minValue, maxValue);

        if (m_numericalDisplay != null)
        {
            int lastValue = 0;
            Int32.TryParse(m_numericalDisplay.text, out lastValue);
            m_numericalDisplay.text = Mathf.RoundToInt(Mathf.Lerp(lastValue, Value, m_lerpValue)).ToString().PadLeft(m_zeroPadding, '0');
        }
        if (m_canvasDisplay != null)
        {
            m_canvasDisplay.fillAmount = Mathf.Lerp(m_canvasDisplay.fillAmount, GetValuePercent(), m_lerpValue);
            if (m_overrideColor)
            {
                m_canvasDisplay.color = DetermineDisplayColor();
            }
        }
    }

    private float GetValuePercent()
    {
        return (maxValue == 0) ? 1 : (float)Value / maxValue;
    }

    private void SetValuePercent(float percent)
    {
        Value = (int)(percent * maxValue);
    }

    private Color DetermineDisplayColor()
    {
        if (m_displayColor.Length == 0)
            return Color.clear;
        if (m_displayColor.Length == 1)
            return m_displayColor[0];
        if (Value == maxValue)
            return m_displayColor[m_displayColor.Length - 1];

        // Find where we are in displayColor[]
        float percent = GetValuePercent();
        int buckets = (m_useColorGradient) ? m_displayColor.Length - 1 : m_displayColor.Length;
        int index = (int)Mathf.Floor(percent * buckets);
        float percentInBucket = (percent * buckets) % 1;

        if (m_useColorGradient)
        {
            return Color.Lerp(m_displayColor[index], m_displayColor[index + 1], percentInBucket);
        }
        else
        {
            return m_displayColor[index];
        }
    }
}
