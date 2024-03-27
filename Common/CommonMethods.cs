
using System.Collections.Generic;
using UnityEngine;

public static class CommonMethods
{

    /// <summary>
    /// Функция рачсета углов в зависимости от осей и заданной точки
    /// 
    /// </summary>
    /// <param name="point">координата позиционирования робота</param>
    /// <param name="axes">оси вращения</param>
    /// <returns></returns>
    public static List<float> GetAngles(Vector3 point, Transform[] axes)
    {

        float b1 = point.x - axes[0].position.x;
        float a1 = point.z - axes[0].position.z;
        float angle = (Mathf.Atan2(b1, a1) + Mathf.PI / 2) * Mathf.Rad2Deg;


        float targetX = Vector2.Distance(Vector2.zero, new Vector2(point.x, point.z));
        float targetY = point.y;


        float c = Vector3.Distance(axes[1].position, point);


        float a = Vector3.Distance(axes[2].position, axes[1].position);
        float b = Vector3.Distance(axes[3].position, axes[2].position);


        float q1 = Mathf.Atan((targetY - axes[1].position.y) / targetX);
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
            //float q4 = Mathf.PI / 2 - angle1;
            angle2 = Mathf.PI - q3;

            angle1 *= Mathf.Rad2Deg;
            angle2 *= Mathf.Rad2Deg;

        }

        if (angle < 0)
            angle += 360;

        return new List<float> { angle, angle1, angle2 };
    }
}