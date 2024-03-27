using System;
using System.Numerics;

class OZK
{
    public static float GetTettas(Matrix4x4 tn, Vector3 point)
    {
        //var p05 = new Vector3(tn.M14, tn.M24, tn.M34);
        //var r05 = GetRotationMatrix(tn) * d5;
        //var p45 = new Vector3(r05.M13, r05.M23, r05.M33);
        //var p03 = p05 - p45;

        //var tetta1 = MathF.Atan2(p03.Y, p03.X);



        //return tetta1;
        throw new NotImplementedException();
    }

    private static Matrix4x4 GetRotationMatrix(Matrix4x4 tn)
    {
        return new Matrix4x4(
            tn.M11, tn.M12, tn.M13, 0, 
            tn.M21, tn.M22, tn.M23, 0, 
            tn.M31, tn.M32, tn.M33, 0, 
            0, 0, 0, 0);
    }
}