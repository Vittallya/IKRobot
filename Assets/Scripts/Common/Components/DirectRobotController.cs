using Assets.Scripts.Common.Core.Constraints;
using Assets.Scripts.Common.Core.Math;
using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Components
{
    public class DirectRobotController : MonoBehaviour
    {
        private PlcConnection _connection;
        private IResolverIK resolver;
        private IAxisesConstraintChecker checker;
        private RobotConfiguration robotConfig;
        private GameObject mock;

        public ConfigurationComponent configuration;

        public Transform[] Axises;

        public Transform RealWorkingInstrument;

        public GameObject Target;

        public Transform DefaultPosition;

        public bool IsRealTime;

        public Transform Axis3Position;

        private void Start()
        {
            checker = new AxisesConstraintChecker();
            robotConfig = configuration.Configuration.RobotConfiguration;
            resolver = new IKResolver5Axises(robotConfig);
            Target.GetComponent<MovableObjectComponent>().TargetMoved.AddListener(OnTargetMoved);
            mock = new GameObject("mock");
        }

        public void SetRealTime(bool value)
        {
            IsRealTime = value;
        }

        private void OnTargetMoved(Vector3 arg0)
        {
            mock.transform.position = arg0;
            mock.transform.rotation = Target.transform.rotation;

            if(CheckReolveIK(mock.transform, out var solutions))
            {
                Target.transform.position = arg0;
                if(IsRealTime)
                    ExecuteSolutions(solutions);
            }
        }

        public void OnDirectModeSelected()
        {
            Target.SetActive(true);
            var pos = _connection != null ? RealWorkingInstrument.position : DefaultPosition.position;
            Target.transform.position = pos;
            Target.transform.rotation = Quaternion.identity;
        }

        public void OnDirectModeDelected()
        {
            Target.SetActive(false);
        }

        public void OnConnected(PlcConnection plcConnection)
        {
            _connection = plcConnection;
        }

        public void OnDisconnected()
        {
            _connection = null;
        }

        private bool CheckReolveIK(Transform target, out List<AxisSolution> solutions)
        {
            solutions = CommonMethods
                .GetAngles(target, Axises, robotConfig)
                .Select(x => new AxisSolution(x, Vector3.zero))
                .ToList();
            //solutions = resolver.ResolveIK(target, Axises).ToList();
            Axis3Position.position = solutions[2].NewPosition;
            //return checker.Check(Axises, solutions, target.position, robotConfig);
            return true;
        }

        public void OnExecute()
        {
            if (CheckReolveIK(Target.transform, out var solutions))
            {
                ExecuteSolutions(solutions);
            }
        }

        private void ExecuteSolutions(List<AxisSolution> solutions)
        {
            for (int i = 0; i < solutions.Count; i++)
            {
                Axises[i].localRotation = Quaternion.Euler(0, solutions[i].NewAngle, 0);
            }
            if (solutions[0].NewAngle < 0)
                solutions[0] = new AxisSolution(solutions[0].NewAngle + 360, solutions[0].NewPosition);
            _connection?.SendToPlc(solutions.Select(x => (object)x.NewAngle), m => Debug.Log(m));
        }
    }
}