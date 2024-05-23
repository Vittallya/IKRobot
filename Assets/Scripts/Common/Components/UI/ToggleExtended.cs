using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Components
{
    public class ToggleExtended : Toggle
    {
        public UnityEvent OnValueTrue;
        public UnityEvent OnValueFalse;

        protected override void Awake()
        {
            onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnValueTrue.Invoke();
            }
            else
            {
                OnValueFalse.Invoke();
            }
        }
    }
}
