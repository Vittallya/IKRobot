using Assets.Scripts.Common.Core.Constraints;
using Assets.Scripts.Common.Core.Interfaces;
using Assets.Scripts.Common.Core.Math;
using Assets.Scripts.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common.Components
{
    public class DirectRobotController : MonoBehaviour
    {
        private PlcConnection _connection;
        private IResolverIK resolver;
        private IAxisesConstraintChecker checker;
        private RobotConfiguration robotConfig;
        private GameObject mock;
        private IValueConverter anglesConverter;
        private IValueConverter axis1AngleConverter;
        private float zAxisRotation;
        private float xAxisRotation;

        public ConfigurationComponent configuration;

        public Transform[] Axises;

        public Transform RealWorkingInstrument;

        public GameObject Target;

        public Transform DefaultPosition;

        public bool IsRealTime;

        public Transform Axis3Position;

        public Material SimulationMaterial;

        public Transform Axis1;

        public IReadOnlyCollection<AxisSolution> CurrentSolutions { get; private set; }

        public UnityEvent<List<AxisSolution>> OnSolutionsApplyed;
        
        private void Start()
        {
            ColorRobot(Color.white, 0.5f);
            checker = new AxisesConstraintChecker();
            robotConfig = configuration.Configuration.RobotConfiguration;
            resolver = new IKResolver5Axises(robotConfig);
            Target.GetComponentInChildren<MovableObjectComponent>().TargetMoved.AddListener(OnTargetMoved);
            var rotatable = Target.GetComponentInChildren<RotatableComponent>();

            rotatable.OnRotated.AddListener(OnTargetRotated);
            rotatable.OnBeginRotating.AddListener(OnTargetBeginRotatig);
            rotatable.OnEndRotating.AddListener(OnTargetEndRotating);
            rotatable.gameObject.SetActive(false);

            mock = new GameObject("mock");
            anglesConverter = new PlcAngleConverter();
            axis1AngleConverter = new Axis1AngleConverter();

            IsRealTime = false;

            ApplyTransform(Target.transform);
        }
        Quaternion targetRotationOnBegin;
        MovingMode lastRotateMode;


        private void OnTargetBeginRotatig(MovingMode mode)
        {
            var pos = Axis1.position;
            pos.y = Target.transform.position.y;
            Target.transform.LookAt(pos);
            Target.transform.rotation = Target.transform.rotation * Quaternion.Euler(xAxisRotation, 90, zAxisRotation);
            targetRotationOnBegin = Target.transform.rotation;
            lastRotateMode = mode;
        }

        private void OnTargetEndRotating()
        {
            zAxisRotation = Target.transform.rotation.eulerAngles.z;
            xAxisRotation = Target.transform.rotation.eulerAngles.x;
        }

        private void OnTargetRotated(float arg0)
        {
            var delta = arg0 * 0.1f;

            var rVector = new Vector3(lastRotateMode == MovingMode.X ? delta : 0, 0, lastRotateMode == MovingMode.Z ? delta : 0);
            mock.transform.position = Target.transform.position;
            Target.transform.rotation = targetRotationOnBegin * Quaternion.Euler(rVector);

            mock.transform.rotation = Target.transform.rotation;
            ApplyTransform(mock.transform);
        }

        private void ApplyTransform(Transform transform)
        {
            if (CheckReolveIK(transform, out var solutions))
            {
                ColorRobot(Color.white, 0.5f);

                if (IsRealTime)
                {
                    SendToPlc(solutions);
                }
            }
            else
            {
                ColorRobot(Color.red, 0.5f);
            }
            CurrentSolutions = solutions;

            ApplySolutionToModel(solutions);
        }

        public void SetRealTime(bool value)
        {
            IsRealTime = value;
        }

        private void OnTargetMoved(Vector3 arg0)
        {
            mock.transform.position = arg0;
            mock.transform.rotation = Target.transform.rotation;
            Target.transform.position = arg0;
            var pos = Axis1.position;
            pos.y = Target.transform.position.y;
            Target.transform.LookAt(pos);
            Target.transform.rotation *= Quaternion.Euler(0, 90, zAxisRotation);

            ApplyTransform(mock.transform) ;
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
            //Axis3Position.position = solutions[2].NewPosition;
            return checker.Check(Axises, solutions, target.position, robotConfig);
        }

        public void OnExecute()
        {
            if (CheckReolveIK(Target.transform, out var solutions))
            {
                ColorRobot(Color.white, 0.5f);
                SendToPlc(solutions);
            }
            else
            {
                ColorRobot(Color.red, 0.5f);
            }

            ApplySolutionToModel(solutions);
        }

        private void ColorRobot(Color col, float alpha)
        {
            SimulationMaterial.color = new Color(col.r, col.g, col.b, alpha);
        }

        private void ApplySolutionToModel(List<AxisSolution> solutions)
        {
            for (int i = 0; i < solutions.Count; i++)
            {
                Axises[i].localRotation = Quaternion.Euler(0, solutions[i].NewAngle, 0);
            }
            if (solutions[0].NewAngle < 0)
                solutions[0] = new AxisSolution(solutions[0].NewAngle + 360, solutions[0].NewPosition);
            OnSolutionsApplyed.Invoke(solutions);
        }



        private void SendToPlc(List<AxisSolution> solutions)
        {
            solutions[0] = new AxisSolution(
                (float)axis1AngleConverter.Convert(solutions[0].NewAngle), solutions[0].NewPosition);

            _connection?.SendToPlc(solutions.Select(x => (object)x.NewAngle), m => Debug.Log(m), anglesConverter);
        }
    }
}