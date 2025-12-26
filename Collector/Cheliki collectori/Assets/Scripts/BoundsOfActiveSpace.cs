using UnityEngine;

public static class BoundsOfActiveSpace
{
    public static float leftBorder { get; private set; } = -4.0f;
    public static float rightBorder { get; private set; } = 4.0f;
    public static float topBorder { get; private set; } = 4.0f;
    public static float bottomBorder { get; private set; } = -4.0f;
}