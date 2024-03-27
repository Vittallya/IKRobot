using System;
using UnityEngine;

[Serializable]
public class RobotState
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
}