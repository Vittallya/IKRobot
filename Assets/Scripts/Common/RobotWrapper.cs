using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
    internal class RobotWrapper : MonoBehaviour
    {
        public Transform[] Axises { get; private set; }
        public Material SimulationMaterial;

        public GameObject Label;

        public int AxisesCount = 5;

        private void Awake()
        {
            Axises = new Transform[AxisesCount];
            var currentTransform = transform;

            for (int i = 0; i < AxisesCount; i++)
            {
                var axis = currentTransform.Find("Axis" + (i + 1));
                currentTransform = axis.Find("Axis" + (i + 2) + "S");
                Axises[i] = axis;
            }
        }

        public void ColorRobot(Color col, float alpha)
        {
            SimulationMaterial.color = new Color(col.r, col.g, col.b, alpha);
        }

        public void ApplySolutionToModel(List<AxisSolution> solutions)
        {
            for (int i = 0; i < solutions.Count; i++)
            {
                Axises[i].localRotation = Quaternion.Euler(0, solutions[i].NewAngle, 0);
            }
            if (solutions[0].NewAngle < 0)
                solutions[0] = new AxisSolution(solutions[0].NewAngle + 360, solutions[0].NewPosition);
        }
    }
}
