using System.Collections.Generic;

namespace Assets.Scripts.Common.Models
{
    public class Route
    {
        public string Name { get; }

        public Route(string name)
        {
            Name = name;
        }

        public List<RobotState> RobotStates { get; set; }
    }
}
