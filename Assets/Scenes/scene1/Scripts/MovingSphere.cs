using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MovingMode
{
    XZ, Y
}

public class MovingSphere : MonoBehaviour
{

    public Texture2D cursor;

    private MovingMode movingMode = MovingMode.Y;

    private Vector3? captured2DPos;
    private Vector3 captured3DPos;
    private Vector3 captured3DPosTarget;

    public CameraAroundComponent CameraAroundComponent;

    private bool isMouseOver;

    public UnityEvent OnTargetMouseEnterEvent;
    public UnityEvent OnMouseUpEvent;


    Vector3? delta = null;

    void Update()
    {
        if (captured2DPos.HasValue)
        {
            // Получаем позицию курсора мыши в экранных координатах
            Vector3 mousePosition = Input.mousePosition;



            Vector3 pos = transform.position;

            if (movingMode == MovingMode.Y)
            {
                var delta = mousePosition - captured2DPos.Value;
                pos.y = captured3DPosTarget.y + delta.y * 0.01f;
            }
            else if(movingMode == MovingMode.XZ)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
                var delta = worldPosition - captured3DPos;
                pos = captured3DPosTarget + delta * 10;
                pos.y = transform.position.y;
            }
            
            transform.position = pos;
        }
    }

    private void OnMouseDown()
    {
        captured3DPosTarget = transform.position;
        captured2DPos = Input.mousePosition;
        captured3DPos = Camera.main.ScreenToWorldPoint(new Vector3(captured2DPos.Value.x, captured2DPos.Value.y, Camera.main.nearClipPlane));
    }

    private void OnMouseOver()
    {
        isMouseOver = true;
    }


    private void OnMouseUp()
    {
        CameraAroundComponent.IsActive = true;
        OnMouseUpEvent?.Invoke();
        captured2DPos = null;
    }

    private void OnMouseEnter()
    {
        CameraAroundComponent.IsActive = false;
        Cursor.SetCursor(cursor, new Vector2(32, 32), CursorMode.Auto);
        OnTargetMouseEnterEvent?.Invoke();
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if(captured2DPos == null)
        {
            CameraAroundComponent.IsActive = true;
        }

        isMouseOver = false;
    }

    public void OnSetXZMode(Toggle toggle)
    {
        RadioButtonsCheckReset(toggle);
        movingMode = MovingMode.XZ;
    }

    private void RadioButtonsCheckReset(Toggle toggle)
    {
        var canv = toggle.GetComponentInParent<Canvas>();
        canv.GetComponentsInChildren<Toggle>().ToList().ForEach(t =>
        {
            t.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        });
        toggle.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
    }

    public void OnSetYMode(Toggle toggle)
    {
        RadioButtonsCheckReset(toggle);
        movingMode = MovingMode.Y;
    }

}
