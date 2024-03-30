using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Core.Constraints
{
    public class AxisesConstraintChecker : IAxisesConstraintChecker
    {
        private readonly IAxisConstraintChecker[] _constraints;

        public AxisesConstraintChecker()
        {
            _constraints = new[]
            {
                new AxisAngleConstraintChecker()
            };
        }

        public bool Check(Transform[] axises, IReadOnlyCollection<AxisSolution> solutions, Vector3 targetPoint, RobotConfiguration robotConfiguration)
        {
            var unions = robotConfiguration.RobotUnions;
            var unionSolutions = unions.Zip(solutions, (u, s) => (u, s));

            return _constraints
                .All(c => unionSolutions
                    .All(solution => c.Check(solution.s.NewPosition, solution.s.NewAngle, solution.u)));
        }
    }
}
