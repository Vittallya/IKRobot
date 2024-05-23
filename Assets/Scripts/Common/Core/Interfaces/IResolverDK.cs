using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Numerics;

public interface IResolverDK
{
    (Vector3, Vector3) Resolve(RobotConfiguration config, List<float> angles);
}