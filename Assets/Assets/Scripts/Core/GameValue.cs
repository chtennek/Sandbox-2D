using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Sandbox.RPG;

public class GameValue : MonoBehaviour
{
    public Stat stat;

    public float minValue = 0;
    public float maxValue = Mathf.Infinity;
    public float m_currentValue = 0; // Initial value
    public bool clampMin = true;
    public bool clampMax = true;
    public bool roundToInt = true;
    public float Value
    {
        get { return m_currentValue; }
        set
        {
            m_lastValue = m_currentValue;
            m_lastValueChange = Time.time;

            m_currentValue = Mathf.Clamp(value, clampMin ? minValue : -Mathf.Infinity, clampMax ? maxValue : Mathf.Infinity);
            if (roundToInt == true)
                m_currentValue = Mathf.Round(m_currentValue);
        }
    }
    public float ValuePercent
    {
        get { return (maxValue == 0) ? 0 : Value / maxValue; }
        set
        {
            Value = value * maxValue;
        }
    }

    [Header("Display")]
    public Ease lerpMethod = Ease.Linear;
    public float m_lerpValueScale = 0;
    public float m_lerpTime = 0.2f;
    public int m_ticksPerSecond = 30;

    [Space]
    public Text m_numericalDisplay;
    public int m_zeroPadding = 0;
    public string m_tickSound; // Sound played on each tick

    [Space]
    public Image m_canvasDisplay;
    public bool m_overrideColor = false;
    public Gradient m_displayColor;

    private float m_lastValueChange;
    private float m_lastValue;

    private void Reset()
    {
        if (m_numericalDisplay == null)
            m_numericalDisplay = GetComponent<Text>();
        if (m_canvasDisplay == null)
            m_canvasDisplay = GetComponent<Image>();
    }

    private void Start()
    {
        m_lastValue = Value; // If we fail to retrieve a display value, default to current Value
        m_lastValueChange = Time.time;

        if (m_numericalDisplay != null)
            Single.TryParse(m_numericalDisplay.text, out m_lastValue);
        else if (m_canvasDisplay != null)
            m_lastValue = m_canvasDisplay.fillAmount * maxValue;

        if (m_numericalDisplay != null || m_canvasDisplay != null)
        {
            IEnumerator current = Coroutine_UpdateDisplay();
            StartCoroutine(current);
        }
    }

    private IEnumerator Coroutine_UpdateDisplay()
    {
        while (true)
        {
            float displayValue = GetDisplayValue();
            if (displayValue != Value)
                AudioManager.PlaySound(m_tickSound);

            if (m_numericalDisplay != null)
            {
                float finalDisplayValue = displayValue;
                if (roundToInt == true)
                    finalDisplayValue = Mathf.Round(finalDisplayValue);

                string text = finalDisplayValue.ToString().PadLeft(m_zeroPadding, '0');
                m_numericalDisplay.text = text;
            }

            if (m_canvasDisplay != null)
            {
                m_canvasDisplay.fillAmount = (maxValue == 0) ? 0 : displayValue / maxValue;

                if (m_overrideColor == true)
                    m_canvasDisplay.color = m_displayColor.Evaluate(ValuePercent);
            }

            yield return new WaitForSeconds(1f / m_ticksPerSecond);
        }
    }

    private float GetDisplayValue()
    {
        float lerpTime = m_lerpTime;
        if (m_lerpValueScale != 0)
            lerpTime *= Mathf.Abs(Value - m_lastValue) / m_lerpValueScale;
        float t = Mathf.Clamp01((Time.time - m_lastValueChange) / lerpTime);
        return DOVirtual.EasedValue(m_lastValue, Value, t, lerpMethod);
    }
}
