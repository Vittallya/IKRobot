using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotConfigurationComponent : Component
{
    public List<RobotUnion> Units;
    private readonly RobotConfiguration config;

    public RobotUnion this[int index] => Units[index];

    public RobotConfigurationComponent(RobotConfiguration config)
    {
        Units = config.RobotUnions.ToList();
        this.config = config;
    }
}
