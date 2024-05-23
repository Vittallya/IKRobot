using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Common.Components
{
    public class RotatableComponent : InteractableObjectComponent
    {
        public float rotationSpeed = 1f;

        public UnityEvent<float> OnRotated;
        public UnityEvent<MovingMode> OnBeginRotating;
        public UnityEvent OnEndRotating;

        public Quaternion RobotOrientation;

        private Vector3 mousePosFixed;

        public MovingMode movingMode;

        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            mousePosFixed = Input.mousePosition;
            OnBeginRotating?.Invoke(movingMode);
        }

        public void SetMode(string mode)
        {
            movingMode = Enum.Parse<MovingMode>(mode);
        }


        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            OnEndRotating?.Invoke();
        }

        private void OnMouseDrag()
        {
            var delta = Input.mousePosition.x - mousePosFixed.x;
            OnRotated.Invoke(delta);
        }

    }
}
