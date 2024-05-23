using System;
using u = UnityEngine;
using n = System.Numerics;

namespace Assets.Scripts.Common.Core.Extensions
{
    public static class QuaternionExtensions
    {
        public static n.Vector3 ToEulerAngles(this n.Quaternion q)
        {
            n.Vector3 angles = new();

            // roll / x
            var sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            var cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = MathF.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            var sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (MathF.Abs(sinp) >= 1)
            {
                var sign = MathF.Sign(sinp);
                angles.Y = sign * MathF.PI / 2;
            }
            else
            {
                angles.Y = MathF.Asin(sinp);
            }

            // yaw / z
            var siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            var cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = MathF.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }
        public static n.Quaternion EulerToQuaternion(this n.Vector3 v)
        {
            var cy = MathF.Cos(v.Z * 0.5f);
            var sy = MathF.Sin(v.Z * 0.5f);
            var cp = MathF.Cos(v.Y * 0.5f);
            var sp = MathF.Sin(v.Y * 0.5f);
            var cr = MathF.Cos(v.X * 0.5f);
            var sr = MathF.Sin(v.X * 0.5f);

            return new n.Quaternion
            {
                W = (cr * cp * cy + sr * sp * sy),
                X = (sr * cp * cy - cr * sp * sy),
                Y = (cr * sp * cy + sr * cp * sy),
                Z = (cr * cp * sy - sr * sp * cy)
            };
        }

        public static n.Quaternion ToSystemQuaternion(this u.Quaternion q)
        {
            return new n.Quaternion
            {
                W = q.w,
                X = q.x,
                Y = q.y,
                Z = q.z,
            };
        }

        public static u.Quaternion ToUnityQuaternion(this n.Quaternion q)
        {
            return new u.Quaternion
            {
                w = q.W,
                x = q.X,
                y = q.Y,
                z = q.Z,
            };
        }
    }
}
