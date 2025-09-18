using System.Collections.Generic;
using UnityEngine;

public enum NamedColor
{
    Grey,
    Black,
    Purple,
    DarkBlue,
    Blue,
    Teal,
    LightGreen,
    Green,
    Yellow,
    Orange,
    Red,
    Pink,
    White
}

public static class ColorMap
{
    public static readonly Dictionary<NamedColor, Color> Palette = new Dictionary<NamedColor, Color>
    {
        {NamedColor.Grey,       new Color(0.373f,0.345f,0.318f)},
        {NamedColor.Black,      new Color(0.400f,0.337f,0.302f)},
        {NamedColor.Purple,     new Color(0.263f,0.294f,0.486f)},
        {NamedColor.DarkBlue,   new Color(0.247f,0.329f,0.459f)},
        {NamedColor.Blue,       new Color(0.192f,0.354f,0.475f)},
        {NamedColor.Teal,       new Color(0.235f,0.404f,0.384f)},
        {NamedColor.LightGreen, new Color(0.304f,0.427f,0.278f)},
        {NamedColor.Green,      new Color(0.322f,0.431f,0.259f)},
        {NamedColor.Yellow,     new Color(0.373f,0.435f,0.192f)},
        {NamedColor.Orange,     new Color(0.529f,0.298f,0.192f)},
        {NamedColor.Red,        new Color(0.651f,0.212f,0.204f)},
        {NamedColor.Pink,       new Color(0.549f,0.200f,0.302f)},
        {NamedColor.White,      new Color(0.337f,0.349f,0.325f)}
    };

    public static NamedColor MapToNearestColor(Color input)
    {
        NamedColor nearestColor = NamedColor.Red; // default
        float minDistance = float.MaxValue;

        foreach (var kvp in Palette)
        {
            Color c = kvp.Value;
            float distance = Vector3.Distance(
                new Vector3(input.r, input.g, input.b),
                new Vector3(c.r, c.g, c.b)
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestColor = kvp.Key;
            }
        }

        return nearestColor;
    }
}
