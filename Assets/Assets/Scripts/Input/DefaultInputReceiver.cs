using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DefaultInputReceiver : InputReceiver
{
    private static List<string> axes;
    private Hashtable buttonDown = new Hashtable();
    private Hashtable buttonUp = new Hashtable();

    private void Awake()
    {
        if (axes == null)
        {
            axes = new List<string>();
            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axisArray = obj.FindProperty("m_Axes");
            for (int i = 0; i < axisArray.arraySize; i++)
            {
                SerializedProperty axis = axisArray.GetArrayElementAtIndex(i);
                if (axis.FindPropertyRelative("type").intValue == 0) // KeyOrMouseButton only
                {
                    axes.Add(axis.FindPropertyRelative("m_Name").stringValue);
                }
            }
        }
    }

    private void Update()
    {
        foreach (string s in axes)
        {
            if (Input.GetButtonDown(s) && !buttonDown.ContainsKey(s))
            {
                buttonDown.Add(s, true);
            }
            else if (Input.GetButtonUp(s) && !buttonUp.ContainsKey(s))
            {
                buttonUp.Add(s, true);
            }
        }
    }

    private void FixedUpdate()
    {
        buttonDown.Clear();
        buttonUp.Clear();
    }

    private string WrapPlayerNum(string id)
    {
        return (playerId + 1).ToString() + ". " + id;
    }

    public override bool GetButtonDownRaw(string id)
    {
        if (!Time.inFixedTimeStep) // This doesn't work
        {
            return Input.GetButtonDown(WrapPlayerNum(id));
        }
        else
        {
            return buttonDown.ContainsKey(WrapPlayerNum(id));
        }
    }

    public override bool GetButtonUpRaw(string id)
    {
        if (!Time.inFixedTimeStep) // This doesn't work
        {
            return Input.GetButtonUp(WrapPlayerNum(id));
        }
        else
        {
            return buttonUp.ContainsKey(WrapPlayerNum(id));
        }
    }

    public override bool GetButtonRaw(string id) { return Input.GetButton(WrapPlayerNum(id)); }
    public override bool GetAnyButtonDownRaw() { return Input.anyKeyDown; }
    public override bool GetAnyButtonRaw() { return Input.anyKey; }

    public override float GetAxisRaw(string id) { return Input.GetAxisRaw(id); }
    public override float GetAxisDownRaw(string id) { return Input.GetAxisRaw(id); } // [TODO]
}
