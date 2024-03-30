using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using UnityEngine;

public static class CommonMethods
{


    public static List<float> GetAngles(Transform target, Transform[] axes, RobotConfiguration config)
    {

        var pointParams = GetAxisPosition2(target, axes, config);
        var point = pointParams.point;

        var pi_2 = Mathf.PI / 2;

        var angle1Rad = Mathf.Atan2(target.position.x - axes[0].position.x, target.position.z - axes[0].position.z) - pi_2;
        var angle = angle1Rad * Mathf.Rad2Deg;

        var targetX = new Vector2(target.position.x, target.position.z).magnitude;
        var pointX = new Vector2(point.x, point.z).magnitude;
        var pointY = point.y;


        var isPositiveArea = point.z * angle1Rad < point.x;

        //Debug.Log(isPositiveArea);
        var isNegativeArea = targetX < config.RobotUnions[4].D * Mathf.Cos(pointParams.angle * Mathf.Deg2Rad);

        var p2 = new Vector2(axes[2].position.x - target.position.x, axes[2].position.z - target.position.z).magnitude;

        var isNegativeArea2 = targetX < p2;

        //var anglePoint = Mathf.Atan2(point.x, point.z);

        //то, что 3 ось находится в отриц. зоне

        //Debug.Log(isNegativeArea2);


        if (isNegativeArea)
        {
            pointX = -pointX;
        }

        float c = Vector3.Distance(axes[1].position, point);


        float a = Vector3.Distance(axes[2].position, axes[1].position);
        float b = Vector3.Distance(axes[3].position, axes[2].position);


        float q1 = Mathf.Atan2(pointY - axes[1].position.y, pointX);

        float angle1;
        float angle2 = 0.0f;

        if (a + b < c)
        {
            angle1 = (- q1) * Mathf.Rad2Deg;
        }
        else
        {
            float q2 = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c));
            float q3 = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));

            angle1 = - q1 - q2;
            angle2 = Mathf.PI - q3;

            angle1 *= Mathf.Rad2Deg;
            angle2 *= Mathf.Rad2Deg;
        }

        if (angle < 0)
            angle += 360;



        var axis2point2d = new Vector2(new Vector2(axes[2].position.x, axes[2].position.z).magnitude, axes[2].position.y);


        var axis3point2d = new Vector2(pointX, point.y);
        var targetPoint2d = new Vector2(targetX, target.position.y);


        if (isNegativeArea2)
        {
            axis2point2d = new Vector2(-axis2point2d.x, axis2point2d.y);
        }

        var a2 = Vector2.Distance(axis2point2d, targetPoint2d);
        var b2 = Vector2.Distance(axis3point2d, axis2point2d);
        var c2 = Vector2.Distance(axis3point2d, targetPoint2d);
        

        var k = (targetPoint2d.y - axis2point2d.y) / (targetPoint2d.x - axis2point2d.x);
        var b3 = axis2point2d.y - k * axis2point2d.x;

        var y1 = k * axis3point2d.x + b3;



        var angle4 = Mathf.Acos((b2 * b2 + c2 * c2 - a2 * a2) / (2 * b2 * c2)) - Mathf.PI;


        //point1 - косяк, когда уходит в отриц область

        //Debug.Log($"a2 = {a2} , b2 = {b2}, c2 = {c2}, k = {k}, b3 = {b3}, y1 = {y1}, axis3point2d.y = {axis3point2d.y}");
        //Debug.Log($"point1 = {axis2point2d} , point2 = {axis3point2d}, point3 = {targetPoint2d}");

        if (y1 < axis3point2d.y)
        {
            //Debug.Log("angle invert");
            angle4 = -angle4;
        }

        //var angle5 = Vector3.Angle(Quaternion.Euler(0, 90, 0) * point, target.rotation * Vector3.right) - 90;
        var angle5 = Vector3.Angle(target.up, Vector3.up);

        return new List<float> { angle, angle1, angle2, angle4 * Mathf.Rad2Deg, angle5 };
    }

    public static Vector3 GetAxisPosition(Transform target, Transform[] axes, RobotConfigurationComponent config)
    {
        var deltaPos = target.position - axes[0].position;
        deltaPos.y = 0;
        var dirVector = Vector3.Normalize(deltaPos);

        var p03 = target.position - target.rotation * dirVector * config.Units[4].D;

        return p03;
    }

    public static (Vector3 point, float angle) GetAxisPosition2(Transform target, Transform[] axes, RobotConfiguration config)
    {
        var deltaPos = target.position - axes[0].position;
        deltaPos.y = 0;
        var dirVector = Vector3.Normalize(deltaPos);


        var v1 = new Vector3(target.position.x, 0, target.position.z);
        var v2 = target.rotation * Vector3.down;
        var angle = Vector3.Angle(v1, v2) - 90;
        //Debug.Log(angle);

        var v3 = Quaternion.Euler(0, 90, 0) * deltaPos;
        var q = Quaternion.AngleAxis(angle, v3);

        var p03 = target.position - q * dirVector * config.RobotUnions[4].D;

        return (p03, angle);
    }
}
