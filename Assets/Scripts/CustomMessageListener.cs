/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * When creating your message listeners you need to implement these two methods:
 *  - OnMessageArrived
 *  - OnConnectionEvent
 */
public class CustomMessageListener : MonoBehaviour
{
    private bool[] tiltStates = new bool[16];
    private float[] tiltDownTime = new float[16];

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        ParseLine(msg);
    }

    public void ParseLine(string line)
    {
        if (line.StartsWith("C:")) // Example: "C:255,128,0"
        {
            //Debug.Log("Color");
            //ParseColor(line.Substring(2));
        }
        else if (line.StartsWith("T")) // Example: "T0:1"
        {
            //Debug.Log(line);
            ParseTilt(line);
        }
    }

    private void ParseColor(string colorString)
    {
        string[] parts = colorString.Split(',');
        if (parts.Length == 3 &&
            byte.TryParse(parts[0], out byte r) &&
            byte.TryParse(parts[1], out byte g) &&
            byte.TryParse(parts[2], out byte b))
        {
            Color color = new Color32(r, g, b, 255);
            NamedColor named = ColorMap.MapToNearestColor(color);
            Debug.Log($"Received color {color}, mapped to {named}");
        }
    }

    private void ParseTilt(string line)
    {
        // Expecting something like "T3:1"
        string[] parts = line.Substring(1).Split(':');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int index) &&
            int.TryParse(parts[1], out int state))
        {
            //the number received when its up is 1023 and when down is around the 80 mark
            bool isDown = state <= 500;
            if (tiltStates[index] != isDown)
            {
                tiltStates[index] = isDown;

                if (isDown)
                    tiltDownTime[index] = Time.time; // record press start
                else
                {
                    float heldDuration = Time.time - tiltDownTime[index];
                    Debug.Log($"Tilt switch {index} held for {heldDuration:F2} seconds");
                    tiltDownTime[index] = 0;
                }
            }
        }
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
}
