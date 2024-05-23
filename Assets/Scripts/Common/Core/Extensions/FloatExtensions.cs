namespace Assets.Scripts.Common.Core.Extensions
{
    public static class FloatExtensions
    {
        const float degree_to_radian = System.MathF.PI / 180f;
        const float radian_to_degree = 180f / System.MathF.PI;

        public static float ToRadian(this float value)
        {
            return value * degree_to_radian;
        }
        public static float ToDegree(this float value)
        {
            return value * radian_to_degree;
        }
    }
}
