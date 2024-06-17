using Assets.Scripts.Common.Core.Interfaces;
using Assets.Scripts.Common.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Components
{
    public class DirectRobotController : MonoBehaviour
    {
        private PlcConnection _connection;
        private IValueConverter anglesConverter;
        private IValueConverter axis1AngleConverter;

        public ConfigurationComponentOld configuration;

        public Transform RealWorkingInstrument;


        public bool IsRealTime;

        public RobotManagement robotManagement;
        
        private void Start()
        {
            robotManagement = GetComponent<RobotManagement>();
            robotManagement.OnSolutionsApplyed.AddListener(OnSolutionsApplyed);
            anglesConverter = new PlcAngleConverter();
            axis1AngleConverter = new Axis1AngleConverter();
            IsRealTime = false;
        }

        private void OnSolutionsApplyed(List<AxisSolution> arg0)
        {
            if (IsRealTime)
            {
                SendToPlc(arg0);
            }
        }

        //public void OnDirectModeSelected()
        //{
        //    Target.SetActive(true);
        //    var pos = _connection != null ? RealWorkingInstrument.position : DefaultPosition.position;
        //    Target.transform.position = pos;
        //    Target.transform.rotation = Quaternion.identity;
        //}

        //public void OnDirectModeDelected()
        //{
        //    Target.SetActive(false);
        //}

        public void OnConnected(PlcConnection plcConnection)
        {
            _connection = plcConnection;
        }

        public void OnDisconnected()
        {
            _connection = null;
        }

        public void OnExecute()
        {
            SendToPlc(robotManagement.CurrentSolutions.ToList());
        }


        public void SendToPlc(List<AxisSolution> solutions)
        {
            _connection?.SendToPlc(solutions.Select(x => (object)x.NewAngle), m => Debug.Log(m), axis1AngleConverter, anglesConverter);
        }
    }
}