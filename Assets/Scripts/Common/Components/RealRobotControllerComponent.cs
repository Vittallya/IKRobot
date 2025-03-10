﻿using Assets.Scripts.Common.Core.Interfaces;
using Grpc.Net.Client;
using S7.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Components
{
    public class RealRobotControllerComponent : MonoBehaviour
    {
        private Coroutine coroutine;
        private IValueConverter converter;

        public Transform[] Axises;
        public Transform[] AxisesVirtual;

        public Transform Target;

        public Transform WorkingInstrument;

        public GameObject realRobotRoot;
        public GameObject virtualRobotRoot;
        private RobotWrapper wrapper;
        private RobotWrapper virtualWrapper;

        private void Start()
        {
            converter = new PlcAngleConverter();
            //wrapper = realRobotRoot.GetComponent<RobotWrapper>();
            //virtualWrapper = virtualRobotRoot.GetComponent<RobotWrapper>();
        }

        public void StartLink(PlcConnection plcConnection)
        {
            coroutine = StartCoroutine(GetEncoderAngles(plcConnection));
        }

        public void CloseLink()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        private void OnDestroy()
        {
            CloseLink();
        }

        public void Sync()
        {
            for (int i = 0; i < Axises.Length; i++)
            {
                AxisesVirtual[i].localRotation = Axises[i].localRotation;
            }
            Target.position = WorkingInstrument.position;
        }

        public float[] Angles { get; } = new float[5];

        IEnumerator GetEncoderAngles(PlcConnection connection)
        {
            if(connection is S7PlcConnection s7PlcConnection)
            {
                var plc = s7PlcConnection.Plc;

                while (true)
                {

                    var val1 = plc.Read(S7.Net.DataType.DataBlock, 1, 20, S7.Net.VarType.LReal, 1);
                    var angle1 = Convert.ToSingle(val1);
                    Angles[0] = angle1;

                    Axises[0].localRotation = Quaternion.Euler(0, angle1, 0);

                    for (int i = 1; i < Axises.Length; i++)
                    {
                        var plcVal = plc.Read(S7.Net.DataType.DataBlock, 1, 20 + i * 8, S7.Net.VarType.LReal, 1);
                        var angle = -Convert.ToSingle(plcVal);
                        Axises[i].localRotation = Quaternion.Euler(0, angle, 0);
                        Angles[i] = angle;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
            }

        }
    }
}