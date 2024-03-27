using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotConfiguration : MonoBehaviour
{
    public List<RobotUnion> Units;

    public RobotUnion this[int index] => Units[index];

    public RobotConfiguration(RobotUnion[] units)
    {
        Units = units.ToList();
    }
}
