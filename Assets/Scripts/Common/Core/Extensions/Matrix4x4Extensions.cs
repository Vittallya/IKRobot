using Assets.Scripts.Common.Core.Math;
using U = UnityEngine;
using N = System.Numerics;

public static class Matrix4x4Extensions
{
    public static Matrix3x3 GetMatrix3x3(this U.Matrix4x4 matrix)
    {
        var i = new N.Vector3(matrix.m00, matrix.m10, matrix.m20);
        var j = new N.Vector3(matrix.m01, matrix.m11, matrix.m21);
        var k = new N.Vector3(matrix.m02, matrix.m12, matrix.m22);

        return new Matrix3x3(i, j, k);
    }

    public static Matrix3x3 GetMatrix3x3(this N.Matrix4x4 matrix)
    {
        var i = new N.Vector3(matrix.M11, matrix.M21, matrix.M31);
        var j = new N.Vector3(matrix.M12, matrix.M22, matrix.M32);
        var k = new N.Vector3(matrix.M13, matrix.M23, matrix.M33);

        return new Matrix3x3(i, j, k);
    }

    public static N.Vector3 GetOffsetVector(this N.Matrix4x4 matrix)
    {
        var v = new N.Vector3(matrix.M14, matrix.M24, matrix.M34);
        return v;
    }
}