﻿using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.Models
{
    [Serializable]
    public class RobotConfiguration
    {
        public List<RobotUnion> RobotUnions { get; set; }
    }
}
