using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Common.Components
{
    public class InteractableObjectComponent : MonoBehaviour
    {
        public UnityEvent<bool> IsAvalilable;
        public Texture2D cursor;


        protected bool mouseDown;
        protected MovingMode _mode;


        protected virtual void OnMouseDown()
        {
            mouseDown = true;
            //zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            //offset = gameObject.transform.position - GetMouseWorldPos();
        }

        protected virtual void OnMouseUp()
        {
            IsAvalilable?.Invoke(true);
            mouseDown = false;
        }

        protected virtual void OnMouseEnter()
        {
            Cursor.SetCursor(cursor, new Vector2(32, 32), CursorMode.Auto);
            IsAvalilable?.Invoke(false);
        }

        protected virtual void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (!mouseDown)
            {
                IsAvalilable?.Invoke(true);
            }
        }
    }
}
