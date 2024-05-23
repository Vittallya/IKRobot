using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Cursor = UnityEngine.Cursor;

namespace Assets.Scripts.Common.Components
{
    public enum MovingMode
    {
        X = 3, 
        Y = 4, 
        Z = 5, 
        XY = 12, 
        XZ = 15, 
        YZ = 20,
    }



    public class MovableObjectComponent : InteractableObjectComponent
    {
        private Vector3 offset;
        private float zCoord;
        Quaternion q;

        public bool IsX;
        public bool IsY = true;
        public bool IsZ = true;

        public UnityEvent<Vector3> TargetMoved;
        public DirectRobotController directController;
        public Transform RelativeTo;

        private Quaternion fixedRotation;

        public Transform Axis1Transform;
        private Vector3 dir;

        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            fixedRotation = Axis1Transform.localRotation;
            offset = gameObject.transform.position - GetMouseWorldPos();
            dir = -Axis1Transform.forward;

            //offset.Set(offset.x * dir.x, offset.y, offset.z * dir.z);


            //Debug.Log();

        }

        private void OnMouseDrag()
        {
            var initial = transform.position;
            var matr = Matrix4x4.Rotate(fixedRotation);

            var mousePos = GetMouseWorldPos();

            var newPos = mousePos + offset;
            //var delta = newPos - initial;
            //newPos.Set(delta.x * dir.x + initial.x, newPos.y, delta.z * dir.z + initial.z);

            newPos.x = (IsX ? newPos.x : initial.x);
            newPos.y = IsY ? newPos.y : initial.y;
            newPos.z = (IsZ ? newPos.z : initial.z);

            TargetMoved?.Invoke(newPos);
        }

        public void SetMode(string mode)
        {
            _mode = Enum.Parse<MovingMode>(mode);
            var val = (int)_mode;
            IsX = val % 3 == 0;
            IsY = val % 4 == 0;
            IsZ = val % 5 == 0;
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = zCoord;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
    }
}
