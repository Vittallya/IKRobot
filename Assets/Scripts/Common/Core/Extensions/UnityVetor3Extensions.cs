using N = System.Numerics;
using U = UnityEngine;
public static class UnityVetor3Extensions
{
    public static N.Vector3 GetNumberics(this U.Vector3 v)
    {
        return new N.Vector3(v.x, v.y, v.z);
    }
    public static U.Vector3 GetUnityVector3(this N.Vector3 v)
    {
        return new U.Vector3(v.X, v.Y, v.Z);
    }
}