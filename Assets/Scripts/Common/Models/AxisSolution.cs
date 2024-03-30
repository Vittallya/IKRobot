using UnityEngine;

namespace Assets.Scripts.Common.Models
{
    /// <summary>
    /// Решение для каждой оси
    /// </summary>
    public class AxisSolution
    {
        public AxisSolution(float newAngle, Vector3 newPosition)
        {
            NewAngle = newAngle;
            NewPosition = newPosition;
        }

        public float NewAngle { get; }
        public Vector3 NewPosition { get; }
    }
}
