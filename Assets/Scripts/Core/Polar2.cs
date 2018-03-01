using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Polar2
{
    public float r;
    public float θ;
    public float O
    {
        get { return (r == 0) ? 0 : θ; }
        set { θ = (value % 360 + 360) % 360; }
    }

    public Polar2(float r, float θ)
    {
        this.r = r;
        this.O = θ;
    }

    public static bool operator ==(Polar2 a, Polar2 b)
    {
        return a.r == b.r && a.O == b.O;
    }

    public static bool operator !=(Polar2 a, Polar2 b)
    {
        return !(a == b);
    }

    public static Polar2 operator +(Polar2 a, Polar2 b)
    {
        Vector2 v1 = a;
        Vector2 v2 = b;
        return v1 + v2;
    }

    public static Polar2 operator -(Polar2 a, Polar2 b)
    {
        Vector2 v1 = a;
        Vector2 v2 = b;
        return v1 - v2;
    }

    public static Polar2 operator *(Polar2 r, float d)
    {
        return new Polar2(r.r * d, r.O);
    }

    public static Polar2 operator /(Polar2 r, float d)
    {
        return new Polar2(r.r / d, r.O);
    }

    public static implicit operator Vector2(Polar2 r)
    {
        return new Vector2(r.r * Mathf.Cos(Mathf.Deg2Rad * r.O), r.r * Mathf.Sin(Mathf.Deg2Rad * r.O));
    }

    public static implicit operator Polar2(Vector2 v)
    {
        if (v.magnitude == 0) return new Polar2(0, 0);
        return new Polar2(v.magnitude, Mathf.Rad2Deg * Mathf.Atan2(v.y, v.x));
    }

    public override bool Equals(object obj)
    {
        var polar = obj as Polar2;
        return polar != null &&
               r == polar.r &&
               O == polar.O;
    }

    public override int GetHashCode()
    {
        var hashCode = -1852417125;
        hashCode = hashCode * -1521134295 + r.GetHashCode();
        hashCode = hashCode * -1521134295 + O.GetHashCode();
        return hashCode;
    }
}

[Serializable]
public class Cylindrical3 : Polar2 // [TODO]
{
    public float z;

    public Cylindrical3(float r, float θ, float z) : base(r, θ)
    {
        this.z = z;
    }
}

[CustomPropertyDrawer(typeof(Polar2), true)]
public class Polar2PropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Use smaller label width
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent("X")).x;

        string[] fields = new string[] { "r", "O", "z" };
        string[] labels = new string[] { "R", "θ", "Z" };
        float fieldWidth = position.width / 3;

        for (int i = 0; i < fields.Length; i++)
        {
            SerializedProperty field = property.FindPropertyRelative(fields[i]);
            if (field == null)
                continue;

            Rect fieldPosition = new Rect(position.x + i * fieldWidth, position.y, fieldWidth, position.height);
            EditorGUI.PropertyField(fieldPosition, field, new GUIContent(fields[i]));
        }

        // Reset values
        EditorGUI.indentLevel = indent;
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.EndProperty();
    }
}