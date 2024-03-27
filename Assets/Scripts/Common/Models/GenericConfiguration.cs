using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common.Models
{
    [Serializable]
    public class GenericConfiguration
    {
        public RobotConfiguration RobotConfiguration { get; set; }
        public List<Route> Routes { get; set; }

        public ConnectionConfiguration ConnectionConfiguration { get; set; }
    }
}
