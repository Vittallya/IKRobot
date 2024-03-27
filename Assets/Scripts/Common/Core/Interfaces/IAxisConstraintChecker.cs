using UnityEngine;

public interface IAxisConstraintChecker
{
    bool Check(Vector3 resolvedPosition, float angle, RobotUnion axis);
}