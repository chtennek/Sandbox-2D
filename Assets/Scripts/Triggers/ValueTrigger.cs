using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueTrigger : Trigger
{
    public GameValue m_value;
    public string m_tagName;

    public float valueMin = -Mathf.Infinity;
    public float valueMax = Mathf.Infinity;

    private void Reset()
    {
        m_value = GetComponentInParent<GameValue>();
    }

    private void Awake()
    {
        m_value = this.GetComponentInTag(m_tagName, m_value);
    }

    protected override void Update()
    {
        if (valueMin <= m_value.Value && m_value.Value <= valueMax)
            Active = true;
        else
            Active = false;

        base.Update();
    }
}
