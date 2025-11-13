using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class MovementData
{
    [Header("Movement Type")]
    public DOType doType;

    [HideInInspector] public Vector2 targetValue;
    [HideInInspector] public float toEnd;

    [HideInInspector] public float jumpPower = 2f;
    [HideInInspector] public int jumpCount = 1;

    [HideInInspector] public Vector2[] pathPoints;
    [HideInInspector] public PathType pathType = PathType.Linear;
    [HideInInspector] public PathMode pathMode = PathMode.Full3D;

    [HideInInspector] public float duration = 1f;
    [HideInInspector] public Ease ease = Ease.Linear;
    [HideInInspector] public float delay = 0f;

    [HideInInspector] public LoopTypeCustom loopType = LoopTypeCustom.Loop;
    [HideInInspector] public int loopCount = -1;

    [HideInInspector] public bool useRelativeValues = false;

    public bool Validate(out string error)
    {
        error = string.Empty;

        if (duration <= 0f)
        {
            error = "Duration must be greater than 0";
            return false;
        }

        if (doType == DOType.Path || doType == DOType.LocalPath)
        {
            if (pathPoints == null || pathPoints.Length < 2)
            {
                error = "Path movement requires at least 2 waypoints";
                return false;
            }
        }

        if (doType == DOType.Jump && jumpCount < 1)
        {
            error = "Jump count must be at least 1";
            return false;
        }

        return true;
    }

    public Vector2 GetTargetPosition(Vector2 currentPosition)
    {
        return useRelativeValues ? currentPosition + targetValue : targetValue;
    }

    public Vector2[] GetPathPoints(Vector2 currentPosition)
    {
        if (pathPoints == null || pathPoints.Length == 0)
            return new Vector2[0];

        if (!useRelativeValues)
            return pathPoints;

        Vector2[] relativePath = new Vector2[pathPoints.Length];
        Vector2 offset = currentPosition;

        for (int i = 0; i < pathPoints.Length; i++)
        {
            relativePath[i] = pathPoints[i] + offset;
        }

        return relativePath;
    }
}

public enum DOType
{
    Move,      // Move in 2D space (XY)
    MoveX,     // Move only on X axis
    MoveY,     // Move only on Y axis
    Jump,      // Jump with arc
    Rotate,    // Rotate in 2D (Z axis)
    Path,      // Follow path in world space
    LocalPath  // Follow path in local space
}

public enum LoopTypeCustom
{
    None,      // Play once, no loop
    Loop,      // Restart from beginning
    PingPong   // Play forward then backward
}

