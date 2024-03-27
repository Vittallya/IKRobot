using UnityEngine;

namespace Assets.Scripts.Common.Core.Constraints
{
    public class AxisPositionConstraintChecker : IAxisConstraintChecker
    {
        public bool Check(Vector3 resolvedPosition, float angle, RobotUnion axis)
        {
            var point = new Vector2(new Vector2(resolvedPosition.x, resolvedPosition.z).magnitude, resolvedPosition.y);
            var polygon = axis.AllowedArea;

            bool isInside = false;
            int j = polygon.Length - 1;
            for (int i = 0; i < polygon.Length; i++)
            {
                if ((polygon[i].y < point.y && polygon[j].y >= point.y
                    || polygon[j].y < point.y && polygon[i].y >= point.y)
                    && (polygon[i].x <= point.x || polygon[j].x <= point.x))
                {
                    if (polygon[i].x + (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < point.x)
                    {
                        isInside = !isInside;
                    }
                }
                j = i;
            }
            return isInside;
        }
    }
}
