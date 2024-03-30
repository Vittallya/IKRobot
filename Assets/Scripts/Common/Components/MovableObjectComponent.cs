using UnityEngine;
using UnityEngine.Events;
using Cursor = UnityEngine.Cursor;

namespace Assets.Scripts.Common.Components
{
    public class MovableObjectComponent : MonoBehaviour
    {
        private Vector3 offset;
        private float zCoord;
        Quaternion q;

        public bool IsX;
        public bool IsY = true;
        public bool IsZ = true;

        public UnityEvent<bool> OnTargetMoving;

        public Texture2D cursor;

        private bool mouseDown;

        private void OnMouseDown()
        {
            mouseDown = true;
            zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            offset = gameObject.transform.position - GetMouseWorldPos();
        }

        private void OnMouseDrag()
        {
            var initial = transform.position;
            var newPos = GetMouseWorldPos() + offset;
            newPos.x = IsX ? newPos.x : initial.x;
            newPos.y = IsY ? newPos.y : initial.y;
            newPos.z = IsZ ? newPos.z : initial.z;

            transform.position = newPos;
        }

        private void OnMouseUp()
        {
            OnTargetMoving?.Invoke(true);
            mouseDown = false;
        }

        private void OnMouseEnter()
        {
            Cursor.SetCursor(cursor, new Vector2(32, 32), CursorMode.Auto);
            OnTargetMoving?.Invoke(false);
        }

        private void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (!mouseDown)
            {
                OnTargetMoving?.Invoke(true);
            }
        }


        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = zCoord;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
    }
}
