using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorChannel
{
    Red,
    Green,
    Blue,
    Alpha,
    Hue,
    Saturation,
    Value,
    L,
    A,
    B,
    None
}

public static class Randomc
{
    public static Color value
    {
        get { return Range(Color.black, Color.white); }
    }

    public static Color valueAlpha
    {
        get { return Range(Color.clear, Color.white); }
    }

    public static Color Range(Gradient g)
    {
        return g.Evaluate(Random.value);
    }

    public static Color Range(Color c1, Color c2)
    {
        return Color32.Lerp(c1, c2, Random.value);
    }

    public static Color ChannelRange(Color color, ColorChannel channel) { return ChannelRange(color, channel, 1); }
    public static Color ChannelRange(Color color, ColorChannel channel, float radius)
    {
        float h, s, v, min, max;
        LABColor lab = LABColor.FromColor(color);
        Color.RGBToHSV(color, out h, out s, out v);
        switch (channel)
        {
            case ColorChannel.Red:
                min = Mathf.Clamp01(color.r - radius);
                max = Mathf.Clamp01(color.r + radius);
                color.r = Random.Range(min, max);
                break;
            case ColorChannel.Green:
                min = Mathf.Clamp01(color.g - radius);
                max = Mathf.Clamp01(color.g + radius);
                color.g = Random.Range(min, max);
                break;
            case ColorChannel.Blue:
                min = Mathf.Clamp01(color.b - radius);
                max = Mathf.Clamp01(color.b + radius);
                color.b = Random.Range(min, max);
                break;
            case ColorChannel.Alpha:
                min = Mathf.Clamp01(color.a - radius);
                max = Mathf.Clamp01(color.a + radius);
                color.a = Random.Range(min, max);
                break;
            case ColorChannel.Hue:
                min = Mathf.Clamp01(h - radius);
                max = Mathf.Clamp01(h + radius);
                color = Color.HSVToRGB(h + Random.Range(min, max), s, v);
                break;
            case ColorChannel.Saturation:
                min = Mathf.Clamp01(s - radius);
                max = Mathf.Clamp01(s + radius);
                color = Color.HSVToRGB(h, s + Random.Range(min, max), v);
                break;
            case ColorChannel.Value:
                min = Mathf.Clamp01(s - radius);
                max = Mathf.Clamp01(s + radius);
                color = Color.HSVToRGB(h, s, v + Random.Range(min, max));
                break;
            case ColorChannel.L:
                min = Mathf.Clamp01(lab.l - radius);
                max = Mathf.Clamp01(lab.l + radius);
                lab.l = Random.Range(-min, max);
                color = LABColor.ToColor(lab);
                break;
            case ColorChannel.A:
                min = Mathf.Clamp01(lab.a - radius);
                max = Mathf.Clamp01(lab.a + radius);
                lab.a = Random.Range(-min, max);
                color = LABColor.ToColor(lab);
                break;
            case ColorChannel.B:
                min = Mathf.Clamp01(lab.b - radius);
                max = Mathf.Clamp01(lab.b + radius);
                lab.b = Random.Range(-min, max);
                color = LABColor.ToColor(lab);
                break;
        }
        return color;
    }

    public static Color LinearOffset(Color color, float radius) { return LinearOffset(color, radius, false); }
    public static Color LinearOffset(Color color, float radius, bool randomizeAlpha)
    {
        float r = Random.Range(color.r - radius, color.r + radius);
        float g = Random.Range(color.g - radius, color.g + radius);
        float b = Random.Range(color.b - radius, color.b + radius);
        float a = !randomizeAlpha ? color.a : Random.Range(color.a - radius, color.a + radius);
        return new Color(r, g, b, a);
    }

    public static Color Offset(Color color, float offset, bool randomizeAlpha)
    {
        float value = (color.r + color.g + color.b) / 3;
        float newValue = Random.Range(value - offset, value + offset);
        float ratio = newValue / value;
        float r = ratio * color.r;
        float g = ratio * color.g;
        float b = ratio * color.b;
        float a = !randomizeAlpha ? color.a : ratio * color.a;
        return new Color(r, g, b, a);
    }

    public static Color[] GradientGrid(int n, Gradient g, float jitter)
    {
        Color[] colors = new Color[n];
        for (int i = 0; i < n; i++)
        {
            colors[i] = g.Evaluate((float)i / n + jitter * Random.value);
        }
        return colors;
    }

    public static Color[] GoldenRatio(int n, Gradient g, float offset)
    {
        Color[] colors = new Color[n];
        for (int i = 0; i < n; i++)
        {
            colors[i] = g.Evaluate((0.618033988749895f * i) % 1 + offset);
        }
        return colors;
    }
}
