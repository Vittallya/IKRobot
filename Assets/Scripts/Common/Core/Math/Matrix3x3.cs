using U = UnityEngine;
using N = System.Numerics;

namespace Assets.Scripts.Common.Core.Math
{
    public readonly struct Matrix3x3
    {
        public Matrix3x3(U.Vector3 col1, U.Vector3 col2, U.Vector3 col3)
            :this(col1.GetNumberics(), col2.GetNumberics(), col3.GetNumberics())
        {
        }

        public Matrix3x3(N.Vector3 col1, N.Vector3 col2, N.Vector3 col3)
        {
            Col1 = col1;
            Col2 = col2;
            Col3 = col3;
        }


        public static U.Vector3 operator * (Matrix3x3 m, U.Vector3 v)
        {
            var result = v.x * m.Col1 + v.y * m.Col2 + v.z * m.Col3;
            return result.GetUnityVector3();
        }

        public static N.Vector3 operator * (Matrix3x3 m, N.Vector3 v)
        {
            var result = v.X * m.Col1 + v.Y * m.Col2 + v.Z * m.Col3;
            return result;
        }

        public N.Vector3 Col1 { get; }
        public N.Vector3 Col2 { get; }
        public N.Vector3 Col3 { get; }
    }
}
