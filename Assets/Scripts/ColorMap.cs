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
    //0.251,0.376,0.349 = Grey
    //0.227,0.396,0.373 = Black
    //0.184,0.314,0.518 = Purple
    //0.114,0.333,0.565 = DarkBlue
    //0.86,0.369,0.541 = Blue
    //0.204,0.463,0.408 = Teal
    //231,471,278 = LightGreen
    //231,482,263 = Green
    //373,435,192 = Yellow
    //529,298,192 = Orange
    //651,212,204 = Red
    //549,200,302 = Pink
    //467,663,604 = White
    public static readonly Dictionary<NamedColor, Color> Palette = new Dictionary<NamedColor, Color>
    {
        {NamedColor.Grey,       new Color(0.341f,0.361f,0.333f)},
        {NamedColor.Black,      new Color(0.396f,0.341f,0.302f)},
        {NamedColor.Purple,     new Color(0.263f,0.294f,0.486f)},
        {NamedColor.DarkBlue,   new Color(0.208f,0.325f,0.506f)},
        {NamedColor.Blue,       new Color(0.157f,0.369f,0.510f)},
        {NamedColor.Teal,       new Color(0.235f,0.404f,0.384f)},
        {NamedColor.LightGreen, new Color(0.282f,0.455f,0.275f)},
        {NamedColor.Green,      new Color(0.306f,0.455f,0.259f)},
        {NamedColor.Yellow,     new Color(0.373f,0.435f,0.192f)},
        {NamedColor.Orange,     new Color(0.529f,0.298f,0.192f)},
        {NamedColor.Red,        new Color(0.651f,0.212f,0.204f)},
        {NamedColor.Pink,       new Color(0.549f,0.200f,0.302f)},
        {NamedColor.White,      new Color(0.333f,0.361f,0.333f)}
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
