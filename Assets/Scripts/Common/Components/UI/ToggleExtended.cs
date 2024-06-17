using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Common.Components
{
    public class ToggleExtended : Toggle
    {
        [SerializeField]
        public UnityEvent OnValueTrue;
        [SerializeField]
        public UnityEvent OnValueFalse;

        public string Test;

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
