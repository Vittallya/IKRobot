using Assets.Scripts.Common.Models;
using UnityEngine;

public interface IAxisesConstraintChecker
{
    bool Check(Transform[] axises, AxisSolution[] solutions, Vector3 targetPoint, RobotConfiguration robotConfiguration);
}