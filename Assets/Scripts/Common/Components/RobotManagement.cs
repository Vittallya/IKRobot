using Assets.Scripts.Common.Core.Constraints;
using Assets.Scripts.Common.Core.Math;
using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common.Components
{
    public class RobotManagement : MonoBehaviour
    {
        private IResolverIK resolver;
        private IAxisesConstraintChecker checker;
        private RobotConfiguration robotConfig;
        private GameObject mock;
        private float zAxisRotation;
        private float xAxisRotation;

        public ConfigurationComponentOld configuration;

        public Transform[] Axises;

        public Transform RealWorkingInstrument;

        public GameObject Target;

        public Transform DefaultPosition;

        public Transform Axis3Position;

        public Material SimulationMaterial;

        public Transform Axis1;

        public GameObject RobotRoot;

        public UnityEvent<List<AxisSolution>> OnSolutionsApplyed;
        public bool IsCorrect { get; private set; }

        public IReadOnlyCollection<AxisSolution> CurrentSolutions { get; private set; }


        private RobotWrapper wrapper;

        private void Start()
        {
            checker = new AxisesConstraintChecker();
            robotConfig = configuration.Configuration.RobotConfiguration;
            resolver = new IKResolver5Axises(robotConfig);

            wrapper = RobotRoot.GetComponent<RobotWrapper>();

            wrapper.ColorRobot(Color.white, 0.5f);
            Target.GetComponentInChildren<MovableObjectComponent>().TargetMoved.AddListener(OnTargetMoved);
            var rotatable = Target.GetComponentInChildren<RotatableComponent>();
            rotatable.OnRotated.AddListener(OnTargetRotated);
            rotatable.OnBeginRotating.AddListener(OnTargetBeginRotatig);
            rotatable.OnEndRotating.AddListener(OnTargetEndRotating);
            rotatable.gameObject.SetActive(false);
            mock = new GameObject("mock");

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
            IsCorrect = CheckReolveIK(transform, out var solutions);

            if (IsCorrect)
            {
                wrapper.ColorRobot(Color.white, 0.5f);
            }
            else
            {
                wrapper.ColorRobot(Color.red, 0.5f);
            }
            CurrentSolutions = solutions;
            OnSolutionsApplyed.Invoke(solutions);
            wrapper.ApplySolutionToModel(solutions);
        }

        private void OnTargetMoved(Vector3 arg0)
        {
            mock.transform.position = arg0;
            mock.transform.rotation = Target.transform.rotation;
            Target.transform.position = arg0;
            var pos = Axis1.position;
            pos.y = Target.transform.position.y;
            Target.transform.LookAt(pos);
            Target.transform.rotation *= Quaternion.Euler(xAxisRotation, 90, zAxisRotation);

            ApplyTransform(mock.transform);
        }

        private bool CheckReolveIK(Transform target, out List<AxisSolution> solutions)
        {
            solutions = CommonMethods
                .GetAngles(target, Axises, robotConfig)
                .Select(x => new AxisSolution(x, Vector3.zero))
                .ToList();
            return checker.Check(Axises, solutions, target.position, robotConfig);
        }

    }
}
