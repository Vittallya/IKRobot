using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common.Components
{
    public class AxisComponent : MonoBehaviour
    {
        public UnityEvent<float> AngleChanged;

        public void ChangeAngle(float newAngle)
        {
            AngleChanged?.Invoke(newAngle);
        }

        public float GetAngle()
            => this.transform.localEulerAngles.y;
    }
}
