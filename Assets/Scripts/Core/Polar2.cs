using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Polar2
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
        this.θ = (θ % 360 + 360) % 360;
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

    public static Polar2 operator *(Polar2 a, float d)
    {
        return new Polar2(a.r * d, a.O);
    }

    public static Polar2 operator /(Polar2 a, float d)
    {
        return new Polar2(a.r / d, a.O);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Polar2))
        {
            return false;
        }

        var polar = (Polar2)obj;
        return r == polar.r &&
               O == polar.O;
    }

    public override int GetHashCode()
    {
        var hashCode = -1852417125;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + r.GetHashCode();
        hashCode = hashCode * -1521134295 + O.GetHashCode();
        return hashCode;
    }

    public static implicit operator Vector2(Polar2 a)
    {
        return new Vector2(a.r * Mathf.Cos(Mathf.Deg2Rad * a.O), a.r * Mathf.Sin(Mathf.Deg2Rad * a.O));
    }

    public static implicit operator Polar2(Vector2 v)
    {
        if (v.magnitude == 0) return new Polar2(0, 0);
        return new Polar2(v.magnitude, Mathf.Rad2Deg * Mathf.Atan2(v.y, v.x));
    }

    public static implicit operator Vector3(Polar2 a)
    {
        return (Vector2)a;
    }

    public static implicit operator Polar2(Vector3 v)
    {
        return (Vector2)v;
    }

    public static implicit operator Cylindrical3(Polar2 a)
    {
        return new Cylindrical3(a.r, a.O, 0);
    }

    public static implicit operator Polar2(Cylindrical3 b)
    {
        return new Polar2(b.r, b.O);
    }
}

[Serializable]
public struct Cylindrical3
{
    public float r;
    public float θ;
    public float z;
    public float O
    {
        get { return (r == 0) ? 0 : θ; }
        set { θ = (value % 360 + 360) % 360; }
    }

    public Cylindrical3(float r, float θ, float z)
    {
        this.r = r;
        this.θ = (θ % 360 + 360) % 360;
        this.z = z;
    }

    public static bool operator ==(Cylindrical3 a, Cylindrical3 b)
    {
        return a.r == b.r && a.O == b.O && a.z == b.z;
    }

    public static bool operator !=(Cylindrical3 a, Cylindrical3 b)
    {
        return !(a == b);
    }

    public static Cylindrical3 operator +(Cylindrical3 a, Cylindrical3 b)
    {
        Vector3 v1 = a;
        Vector3 v2 = b;
        return v1 + v2;
    }

    public static Cylindrical3 operator -(Cylindrical3 a, Cylindrical3 b)
    {
        Vector3 v1 = a;
        Vector3 v2 = b;
        return v1 - v2;
    }

    public static Cylindrical3 operator *(Cylindrical3 a, float d)
    {
        return new Cylindrical3(a.r * d, a.O, a.z * d);
    }

    public static Cylindrical3 operator /(Cylindrical3 a, float d)
    {
        return new Cylindrical3(a.r / d, a.O, a.z / d);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Cylindrical3))
        {
            return false;
        }

        var cylindrical = (Cylindrical3)obj;
        return r == cylindrical.r &&
               z == cylindrical.z &&
               O == cylindrical.O;
    }

    public override int GetHashCode()
    {
        var hashCode = 815684312;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + r.GetHashCode();
        hashCode = hashCode * -1521134295 + z.GetHashCode();
        hashCode = hashCode * -1521134295 + O.GetHashCode();
        return hashCode;
    }

    public static implicit operator Vector3(Cylindrical3 a)
    {
        Vector3 v = (Polar2)a;
        v.z = a.z;
        return v;
    }

    public static implicit operator Cylindrical3(Vector3 v)
    {
        Cylindrical3 a = (Vector2)v;
        a.z = v.z;
        return a;
    }

    public static implicit operator Vector2(Cylindrical3 a)
    {
        return (Vector3)a;
    }

    public static implicit operator Cylindrical3(Vector2 v)
    {
        return (Vector3)v;
    }
}

[CustomPropertyDrawer(typeof(Polar2), true)]
[CustomPropertyDrawer(typeof(Cylindrical3), true)]
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

        string[] fields = new string[] { "r", "θ", "z" };
        string[] labels = new string[] { "R", "θ", "Z" };
        float fieldWidth = position.width / 3;

        for (int i = 0; i < fields.Length; i++)
        {
            SerializedProperty field = property.FindPropertyRelative(fields[i]);
            if (field == null)
                continue;

            Rect fieldPosition = new Rect(position.x + i * fieldWidth, position.y, fieldWidth, position.height);
            EditorGUI.PropertyField(fieldPosition, field, new GUIContent(labels[i]));
        }

        // Reset values
        EditorGUI.indentLevel = indent;
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.EndProperty();
    }
}