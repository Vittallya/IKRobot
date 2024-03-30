using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using UnityEngine;

public interface IAxisesConstraintChecker
{
    bool Check(Transform[] axises, IReadOnlyCollection<AxisSolution> solutions, Vector3 targetPoint, RobotConfiguration robotConfiguration);
}