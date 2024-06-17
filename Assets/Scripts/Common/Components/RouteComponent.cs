using Assets.Scripts.Common.Components;
using Assets.Scripts.Common.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class RouteComponent : MonoBehaviour
    {
        public RobotManagement robotManagement;
        public DirectRobotController controller;
        public RealRobotControllerComponent realRobotComponent;
        public GameObject Prefab;
        public GameObject robotRoot;

        private readonly Queue<IReadOnlyCollection<AxisSolution>> _routeSolutions;
        private readonly List<GameObject> _robotPositions;
        private Coroutine coroutine;

        public GameObject realRobotRoot;

        private RobotWrapper realRobotWrapper;

        public RouteComponent()
        {
            _routeSolutions = new Queue<IReadOnlyCollection<AxisSolution>>();
            _robotPositions = new List<GameObject>();
        }
        private void Awake()
        {
            realRobotWrapper = realRobotRoot.GetComponent<RobotWrapper>();
        }

        public bool IsExecuting { get; private set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FixCurrentState();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                Clear();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                StartExecuteRoute();
            }
        }

        public void StartExecuteRoute()
        {
            coroutine = StartCoroutine(ExecuteRoute());
        }

        private IEnumerator ExecuteRoute()
        {
            int count = 0;
            while (_routeSolutions.TryDequeue(out var solutions))
            {
                IsExecuting = true;

                var solutionsList = solutions.ToList();
                controller.SendToPlc(solutionsList);

                var fake = _robotPositions[count++];
                var fakeWrapper = fake.GetComponent<RobotWrapper>();

                while(!IsAxisesEqual(fakeWrapper.Axises, realRobotWrapper.Axises))
                {
                    yield return new WaitForSeconds(0.01f);
                }

                Destroy(fake);
            }
            IsExecuting = false;
        }

        private bool IsAxisesEqual(Transform[] robot1, Transform[] robot2)
        {
            for (int i = 0; i < robot1.Length; i++)
            {
                var axisRobot1Y = robot1[i].localEulerAngles.y;
                var axisRobot2Y = robot2[i].localEulerAngles.y;

                if (Mathf.Abs(axisRobot1Y - axisRobot2Y) > 0.01)
                    return false;

            }
            return true;
        }

        public void FixCurrentState()
        {
            if (robotManagement.IsCorrect)
            {
                _routeSolutions.Enqueue(robotManagement.CurrentSolutions);
                var gameObject = Instantiate(Prefab);
                gameObject.transform.position = robotRoot.transform.position;
                var wrapper =
                gameObject
                    .GetComponent<RobotWrapper>();

                wrapper.ApplySolutionToModel(robotManagement.CurrentSolutions.ToList());

                _robotPositions.Add(gameObject);

                var label = wrapper.Label;
                label.gameObject.SetActive(true);
                label.GetComponent<TextMeshPro>().text =
                    _robotPositions.Count.ToString();

            }
        }

        private void OnDestroy()
        {
            _robotPositions.ForEach(r => Destroy(r));
            StopCoroutine(coroutine);
            IsExecuting = false;
        }

        public void Clear()
        {
            _routeSolutions.Clear();
            StopCoroutine(coroutine);
            _robotPositions.ForEach(r => Destroy(r));
            IsExecuting = false;
        }
    }
}
