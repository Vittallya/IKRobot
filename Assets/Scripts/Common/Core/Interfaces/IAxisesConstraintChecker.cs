using UnityEngine;

public interface IAxisesConstraintChecker
{
    bool Check(Vector3 resolvedPosition, float angle, RobotUnion axis);
}