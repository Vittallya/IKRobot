using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IResolverIK
{
    IReadOnlyCollection<AxisSolution> ResolveIK(Transform target, Transform[] axises);
}