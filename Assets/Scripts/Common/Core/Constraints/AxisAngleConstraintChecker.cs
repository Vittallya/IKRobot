using UnityEngine;

namespace Assets.Scripts.Common.Core.Constraints
{
    public class AxisAngleConstraintChecker : IAxisConstraintChecker
    {
        public bool Check(Vector3 resolvedPosition, float angle, RobotUnion axis)
        {
            return axis.LimitMinDeg < angle && angle < axis.LimitMaxDeg;
        }
    }
}
