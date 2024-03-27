using System;
using UnityEngine;

[Serializable]
public struct RobotUnion
{
    public float A;
    public float Alpha;
    public float D;
    public float TettaOffset;
    public float LimitMinDeg;
    public float LimitMaxDeg;

    /// <summary>
    /// Разрешенная область работы
    /// </summary>
    public Vector2[] AllowedArea;
    public readonly float TettaRad => Mathf.Deg2Rad * TettaOffset;
    public readonly float AlphaRad => Mathf.Deg2Rad * Alpha;
    public float GetTettaRad(float angleRad) => angleRad + TettaOffset;
}
