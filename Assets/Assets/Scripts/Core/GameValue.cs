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
    public bool clampMin = true;
    public bool clampMax = true;
    public bool roundToInt = true;
    public float Value
    {
        get { return m_currentValue; }
        set
        {
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
    public float m_lerpRatio = 0f;
    public float m_lerpSpeed = 1; // Change display in [Value] amount per tick
    public int m_lerpTick = 1; // Number of frames per tick

    [Space]
    public Text m_numericalDisplay;
    public int m_zeroPadding = 0;

    [Space]
    public Image m_canvasDisplay;
    public bool m_overrideColor = false;
    public Gradient m_displayColor;

    private float m_displayValue;

    private void Reset()
    {
        if (m_numericalDisplay == null)
            m_numericalDisplay = GetComponent<Text>();
        if (m_canvasDisplay == null)
            m_canvasDisplay = GetComponent<Image>();
    }

    private void Start()
    {
        m_displayValue = Value; // If we fail to retrieve a display value, set to Value immediately
        if (m_numericalDisplay != null)
        {
            Single.TryParse(m_numericalDisplay.text, out m_displayValue);
        }
        else if (m_canvasDisplay != null)
        {
            m_displayValue = m_canvasDisplay.fillAmount * maxValue;
        }

        IEnumerator current = Coroutine_UpdateDisplay();
        StartCoroutine(current);
    }

    private IEnumerator Coroutine_UpdateDisplay()
    {
        while (true)
        {
            m_displayValue = DetermineNextDisplayValue(m_displayValue);

            if (m_numericalDisplay != null)
            {
                float finalDisplayValue = m_displayValue;
                if (roundToInt == true)
                    finalDisplayValue = Mathf.Round(finalDisplayValue);

                m_numericalDisplay.text = finalDisplayValue.ToString().PadLeft(m_zeroPadding, '0');
            }

            if (m_canvasDisplay != null)
            {
                m_canvasDisplay.fillAmount = (maxValue == 0) ? 0 : m_displayValue / maxValue;

                if (m_overrideColor == true)
                {
                    m_canvasDisplay.color = m_displayColor.Evaluate(ValuePercent);
                }
            }

            for (int i = 0; i < m_lerpTick; i++)
            {
                yield return null;
            }
        }
    }

    private float DetermineNextDisplayValue(float lastValue)
    {
        float nextValue = Mathf.Lerp(lastValue, Value, m_lerpRatio);
        Debug.Log(new Vector3(Value, lastValue, nextValue));

        if (Mathf.Abs(Value - nextValue) <= m_lerpSpeed)
            return Value;
        else if (nextValue < Value)
            return nextValue + m_lerpSpeed;
        else
            return nextValue - m_lerpSpeed;
    }
}
