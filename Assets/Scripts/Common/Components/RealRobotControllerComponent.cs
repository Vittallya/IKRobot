using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Components
{
    public class RealRobotControllerComponent : MonoBehaviour
    {
        private Coroutine coroutine;

        public Transform[] Axises;

        public void StartLink(PlcConnection plcConnection)
        {
            coroutine = StartCoroutine(GetEncoderAngles(plcConnection));
        }

        public void CloseLink()
        {
            StopCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            CloseLink();
        }


        IEnumerator GetEncoderAngles(PlcConnection connection)
        {
            while (true)
            {
                var angles = connection
                    .GetFromPlc(x => Debug.Log(x))
                    .Cast<float>()
                    .ToList();

                Axises[0].rotation = Quaternion.Euler(0, angles[0], 0);

                for (int i = 1; i < Axises.Length; i++)
                {
                    Axises[i].localRotation = Quaternion.Euler(0, angles[i], 0);
                }
            }
        }
    }
}