using System.Collections.Generic;
using UnityEngine;

public enum NamedColor
{
    Red,
    Green,
    Blue,
    Yellow,
    White,
    Black
}

public static class ColorMap
{
    public static readonly Dictionary<NamedColor, Color> Palette = new Dictionary<NamedColor, Color>
    {
        {NamedColor.Red,    Color.red},
        {NamedColor.Green,  Color.green},
        {NamedColor.Blue,   Color.blue},
        {NamedColor.Yellow, Color.yellow},
        {NamedColor.White,  Color.white},
        {NamedColor.Black,  Color.black}
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
