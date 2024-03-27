using System;
using System.Numerics;

class PZK
{
    public const float PI_2 = MathF.PI / 2;

    public static Matrix4x4 GetT1(float angle1, float d1) =>
        new(
                        MathF.Cos(angle1), 0, MathF.Sin(angle1), 0,
                        MathF.Sin(angle1), 0, -MathF.Cos(angle1), 0,
                        0, 1, 0, d1,
                        0, 0, 0, 1
            );

    public static Matrix4x4 GetT2(float angle2, float a2) =>
        new(
                        MathF.Cos(angle2), -MathF.Sin(angle2), 0, a2*MathF.Cos(a2),
                        MathF.Sin(angle2), MathF.Cos(angle2), 0, a2 * MathF.Sin(a2),
                        0, 0, 1, 0,
                        0, 0, 0, 1
            );
    public static Matrix4x4 GetT3(float angle3, float a3) => GetT2(angle3, a3);
    public static Matrix4x4 GetT4(float angle4) =>
        new(
                        MathF.Cos(angle4 + PI_2), 0, MathF.Sin(angle4 + PI_2), 0,
                        MathF.Sin(angle4), 0, -MathF.Cos(angle4 + PI_2), 0,
                        0, 1, 0, 0,
                        0, 0, 0, 1
            );
    public static Matrix4x4 GetT5(float angle5, float d5) =>
        new(
                        MathF.Cos(angle5), -MathF.Sin(angle5), 0, 0,
                        MathF.Sin(angle5), MathF.Cos(angle5), 0, 0,
                        0, 0, 1, d5,
                        0, 0, 0, 1
            );


    /// <summary>
    /// 
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <param name="a3"></param>
    /// <param name="a4"></param>
    /// <param name="a5"></param>
    /// <returns>v1 - вектор смещени€, v2 - углы Ёйлера</returns>
    public static (Vector3, Vector3) ResolvePZK(float an1, float an2, float an3, float an4, float an5, 
        float d1, float a2, float a3, float a4, float d5)
    {
        Matrix4x4 tMatr = GetTnMatrix(an1, an2, an3, an4, an5, d1, a2, a3, d5);
        var v1 = new Vector3(tMatr.M14, tMatr.M24, tMatr.M34);
        var costetta = tMatr.M33;

        var tetta = 0f;
        var fi = 0f;
        var psi = 0f;

        if (MathF.Abs(costetta) < 1)
        {
            var sintetta = MathF.Sqrt(1 - costetta * costetta);
            tetta = MathF.Atan2(sintetta, costetta);
            fi = MathF.Atan2(tMatr.M23, tMatr.M13);
            psi = MathF.Atan2(tMatr.M32, -tMatr.M13);
        }
        else if (1 - costetta < float.Epsilon)
        {
            fi = MathF.Atan2(tMatr.M21, tMatr.M11);
        }
        else
        {
            tetta = MathF.PI;
            fi = MathF.Atan2(-tMatr.M12, -tMatr.M11);
        }

        var v2 = new Vector3(tetta, fi, psi);

        return (v1, v2);
    }

    private static Matrix4x4 GetTnMatrix(float an1,
                                         float an2,
                                         float an3,
                                         float an4,
                                         float an5,
                                         float d1,
                                         float a2,
                                         float a3,
                                         float d5)
        => GetT1(an1, d1) * GetT2(an2, a2) * GetT3(an3, a3) * GetT4(an4) * GetT5(an5, d5);
}