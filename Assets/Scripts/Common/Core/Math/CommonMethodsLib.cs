using System;
using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts.Common.Core.Extensions;

namespace Assets.Scripts.Common.Core.Math
{
    public static class CommonMethodsLib
    {
        public static List<float> GetAngles(Vector3 targetPosition, Quaternion targetRotation, Vector3[] axisesPosition, RobotConfiguration config)
        {

            var pointParams = GetAxisPosition2(targetPosition, targetRotation, axisesPosition, config);
            var point = pointParams.point;

            var pi_2 = MathF.PI / 2;

            var angle1Rad = MathF.Atan2(targetPosition.X - axisesPosition[0].X, targetPosition.Z - axisesPosition[0].Z) - pi_2;
            var angle = angle1Rad.ToDegree();

            var targetX = new Vector2(targetPosition.X, targetPosition.Z).Length();
            var pointX = new Vector2(point.X, point.Z).Length();
            var pointY = point.Y;


            var isNegativeArea = targetX < config.RobotUnions[4].D * MathF.Cos(pointParams.angle.ToRadian());

            var p2 = new Vector2(axisesPosition[2].X - targetPosition.X, axisesPosition[2].Z - targetPosition.Z).Length();

            var isNegativeArea2 = targetX < p2;

            if (isNegativeArea)
            {
                pointX = -pointX;
            }

            float c = Vector3.Distance(axisesPosition[1], point);

            float a = Vector3.Distance(axisesPosition[2], axisesPosition[1]);
            float b = Vector3.Distance(axisesPosition[3], axisesPosition[2]);


            float q1 = MathF.Atan2(pointY - axisesPosition[1].Y, pointX);

            float angle1;
            float angle2 = 0.0f;

            if (a + b < c)
            {
                angle1 = (-q1).ToDegree();
            }
            else
            {
                float q2 = MathF.Acos((a * a + c * c - b * b) / (2 * a * c));
                float q3 = MathF.Acos((a * a + b * b - c * c) / (2 * a * b));

                angle1 = -q1 - q2;
                angle2 = MathF.PI - q3;

                angle1 = angle1.ToDegree();
                angle2 = angle2.ToDegree();
            }

            var axis2point2d = new Vector2(new Vector2(axisesPosition[2].X, axisesPosition[2].Z).Length(), axisesPosition[2].Y);


            var axis3point2d = new Vector2(pointX, point.Y);
            var targetPoint2d = new Vector2(targetX, targetPosition.Y);


            if (isNegativeArea2)
            {
                axis2point2d = new Vector2(-axis2point2d.X, axis2point2d.Y);
            }

            var b2 = Vector2.Distance(axis2point2d, targetPoint2d);
            var a3 = Vector2.Distance(axis3point2d, axis2point2d);
            var d5 = Vector2.Distance(axis3point2d, targetPoint2d);

            //var a3 = config.RobotUnions[2].A;
            //var d5 = config.RobotUnions[4].D;


            var k = (targetPoint2d.Y - axis2point2d.Y) / (targetPoint2d.X - axis2point2d.X);
            var b3 = axis2point2d.Y - k * axis2point2d.X;

            var y1 = k * axis3point2d.X + b3;



            var angle4 = MathF.Acos((a3 * a3 + d5 * d5 - b2 * b2) / (2 * a3 * d5)) - MathF.PI;

            if (y1 < axis3point2d.Y)
            {
                //Debug.Log("angle invert");
                angle4 = -angle4;
            }

            var angle5 = targetRotation.ToEulerAngles().X;

            return new List<float> { angle, angle1, angle2, angle4.ToDegree(), angle5 };
        }

        public static (Vector3 point, float angle) GetAxisPosition2(Vector3 targetPosition, Quaternion targetRotation, Vector3[] axisesPosition, RobotConfiguration config)
        {
            throw new NotImplementedException();
            //var deltaPos = targetPosition - axisesPosition[0];
            //deltaPos.Y = 0;
            //var dirVector = Vector3.Normalize(deltaPos);


            //var v1 = new Vector3(targetPosition.X, 0, target.position.z);
            //var v2 = target.rotation * Vector3.down;
            //var angle = Vector3.Angle(v1, v2) - 90;
            ////Debug.Log(angle);

            //var v3 = Quaternion.Euler(0, 90, 0) * deltaPos;
            //var q = Quaternion.AngleAxis(angle, v3);

            //var p03 = target.position - q * dirVector * config.RobotUnions[4].D;

            //return (p03, angle);
        }
    }
}
