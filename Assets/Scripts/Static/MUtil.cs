using UnityEngine;

/// <summary>
/// Things we'll do in this class
/// Make Mathematic movement with the instance you wanna apply. Use these functions on the ship with localPosition.
/// Utilities.
/// </summary>
static public class MUtil
{
    /// <summary>
    /// Applies Sine function to the Y axis.
    /// </summary>
    /// <returns>The at y.</returns>
    /// <param name="amplitude">Amplitude.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    /// <param name="t">Time.</param>
    /// <param name="speed">Speed.</param>
    static public Vector3 SineAtY(float amplitude, float x, float y, float z, float t, float speed = 2.0f)
    {
        Vector3 pos = new Vector3(x, y, z);
        pos.x = x;
        pos.y = amplitude * Mathf.Sin(y - t * Mathf.PI * speed);
        pos.z = z;
        return pos;
    }

    /// <summary>
    /// Applies Cosen function to the X axis.
    /// </summary>
    /// <returns>The at x.</returns>
    /// <param name="amplitude">Amplitude.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    /// <param name="t">Time.</param>
    /// <param name="speed">Speed.</param>
    static public Vector3 CosAtX(float amplitude, float x, float y, float z, float t, float speed = 2.0f)
    {
        Vector3 pos = new Vector3(x, y, z);
        pos.x = amplitude * Mathf.Cos(x + t * Mathf.PI * speed);
        pos.y = y;
        pos.z = z;
        return pos;
    }

    /// <summary>
    /// Applies Cosen function to the X axis.
    /// </summary>
    /// <returns>The at x.</returns>
    /// <param name="amplitude">Amplitude.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    /// <param name="t">Time.</param>
    /// <param name="speed">Speed.</param>
    static public Vector3 CosAtY(float amplitude, float x, float y, float z, float t, float speed = 2.0f)
    {
        Vector3 pos = new Vector3(x, y, z);
        pos.x = x;
        pos.y = amplitude * Mathf.Cos(y + t * Mathf.PI * speed);
        pos.z = z;
        return pos;
    }

    /// <summary>
    /// Applies Sine function to the X axis.
    /// </summary>
    /// <returns>The at x.</returns>
    /// <param name="amplitude">Amplitude.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    /// <param name="t">Time.</param>
    /// <param name="speed">Speed.</param>
    static public Vector3 SineAtX(float amplitude, float x, float y, float z, float t, float speed = 2.0f)
    {
        Vector3 pos = new Vector3(x, y, z);
        pos.x = amplitude * Mathf.Sin(x - t * Mathf.PI * speed);
        pos.y = y;
        pos.z = z;
        return pos;
    }

    /// <summary>
    /// From float to bool.
    /// </summary>
    /// <returns>The float</returns>
    /// <param name="conversion">If set to <c>true</c> 1.0f. else 0.0f</param>
    static public float BoolF(bool conversion)
    {
        return conversion ? 1.0f : 0.0f;
    }
}
