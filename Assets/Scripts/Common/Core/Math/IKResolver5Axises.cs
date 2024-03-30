using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Core.Math
{
    public class IKResolver5Axises : IResolverIK
    {
        private readonly RobotConfiguration _config;

        public IKResolver5Axises(RobotConfiguration robotConfiguration)
        {
            this._config = robotConfiguration;
        }

        public Vector3 GetAxis2Position(Transform target, Transform[] axes, float angle)
        {
            var deltaPos = target.position - axes[0].position;
            deltaPos.y = 0;
            var dirVector = Vector3.Normalize(deltaPos);

            var v3 = Quaternion.Euler(0, 90, 0) * deltaPos;
            var q = Quaternion.AngleAxis(angle, v3);

            var p03 = target.position - q * dirVector * _config.RobotUnions[4].D;

            return p03;
        }

        private float GetAngleForAxis2(Transform target)
        {
            var v1 = new Vector3(target.position.x, 0, target.position.z);
            var v2 = target.rotation * Vector3.down;
            var angle = Vector3.Angle(v1, v2) - 90;
            return angle;
        }

        public IReadOnlyCollection<AxisSolution> ResolveByPosition(Transform target, Transform[] axes, float angleForAxis3, Vector3 axis3Position)
        {
            var angle1Rad = Mathf.Atan2(target.position.x - axes[0].position.x, target.position.z - axes[0].position.z) - Mathf.PI / 2;
            var angle1 = angle1Rad * Mathf.Rad2Deg;

            var targetX = new Vector2(target.position.x, target.position.z).magnitude;
            var pointX = new Vector2(axis3Position.x, axis3Position.z).magnitude;
            var pointY = axis3Position.y;

            var isNegativeArea = targetX < _config.RobotUnions[4].D * Mathf.Cos(angleForAxis3 * Mathf.Deg2Rad);

            if (isNegativeArea)
            {
                pointX = -pointX;
            }

            float a = Vector3.Distance(axes[2].position, axes[1].position);
            float b = Vector3.Distance(axes[3].position, axes[2].position);
            float c = Vector3.Distance(axes[1].position, axis3Position);

            float q1 = Mathf.Atan2(pointY - axes[1].position.y, pointX);

            float angle2;
            float angle3 = 0.0f;

            if (a + b < c)
            {
                angle2 = (-q1) * Mathf.Rad2Deg;
            }
            else
            {
                float q2 = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c));
                float q3 = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));

                angle2 = -q1 - q2;
                angle3 = Mathf.PI - q3;

                angle2 *= Mathf.Rad2Deg;
                angle3 *= Mathf.Rad2Deg;
            }



            var t12 = PZK.GetT1(angle2, _config.RobotUnions[0].D);
            var axis2Position = t12.GetMatrix3x3() * axis3Position - t12.GetOffsetVector().GetUnityVector3();

            return new List<AxisSolution>
            {
                new(angle1, Vector3.zero),
                new(angle2, axis2Position),
                new(angle3, axis3Position),
            };
        }

        public IReadOnlyCollection<AxisSolution> ResolveByRotation(Transform target, Transform[] axes, Vector3 axis3Position)
        {
            var targetX = new Vector2(target.position.x, target.position.z).magnitude;
            var pointX = new Vector2(axis3Position.x, axis3Position.z).magnitude;

            var p2 = new Vector2(axes[2].position.x - target.position.x, axes[2].position.z - target.position.z).magnitude;

            var isNegativeArea2 = targetX < p2;

            var axis2point2d = new Vector2(new Vector2(axes[2].position.x, axes[2].position.z).magnitude, axes[2].position.y);


            var axis3point2d = new Vector2(pointX, axis3Position.y);
            var targetPoint2d = new Vector2(targetX, target.position.y);


            if (isNegativeArea2)
            {
                axis2point2d = new Vector2(-axis2point2d.x, axis2point2d.y);
            }

            var a = Vector2.Distance(axis2point2d, targetPoint2d);
            var b = Vector2.Distance(axis3point2d, axis2point2d);
            var c = Vector2.Distance(axis3point2d, targetPoint2d);


            var k = (targetPoint2d.y - axis2point2d.y) / (targetPoint2d.x - axis2point2d.x);
            var b3 = axis2point2d.y - k * axis2point2d.x;

            var y1 = k * axis3point2d.x + b3;

            var angle4 = Mathf.Acos((b * b + c * c - a * a) / (2 * b * c)) - Mathf.PI;

            if (y1 < axis3point2d.y)
            {
                angle4 = -angle4;
            }

            var angle5 = Vector3.Angle(target.up, Vector3.up);

            var t45Matr = PZK.GetT4(angle4) * PZK.GetT5(angle5, _config.RobotUnions[4].D);
            var axis4Position = t45Matr.GetMatrix3x3() * target.position - t45Matr.GetOffsetVector().GetUnityVector3();

            return new List<AxisSolution>
            {
                new(angle4, axis4Position),
                new(angle5, target.position)
            };
        }

        public IReadOnlyCollection<AxisSolution> ResolveIK(Transform target, Transform[] axises)
        {
            var angleForAxis3 = GetAngleForAxis2(target);
            var axis3TargetPositon = GetAxis2Position(target, axises, angleForAxis3);

            var resolveByPositon = ResolveByPosition(target, axises, angleForAxis3, axis3TargetPositon);
            var resolveByRotation = ResolveByRotation(target, axises, axis3TargetPositon);

            return resolveByPositon.Union(resolveByRotation).ToList();
        }
    }
}
