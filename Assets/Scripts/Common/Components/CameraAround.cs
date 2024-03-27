using System;
using UnityEngine;

public class CameraAround : MonoBehaviour
{
    public Transform target;

    private Vector3 preview;
    private Vector3 previewOffset;

    public float currentDistanse = 10f;

    public float moveXSensivity = 10f;
    public float moveYSensivity = 10f;

    [Range(-1, 1)]
    public float distanceDir = 0.5f;

    public bool IsActiveUpdatePosition { get; set; } = true;

    private void Start()
    {
        transform.LookAt(target);
        transform.Translate(currentDistanse * Vector3.back);
    }


    private void Update()
    {
        if (IsActiveUpdatePosition)
        {
            UpdateCamPosition();
        }
        UpdateCamScroll();
        UpdateCamOffset();
        
    }

    private void UpdateCamOffset()
    {
        if (Input.GetMouseButtonDown(2))
        {
            previewOffset = GetViewPortCoords();
        }

        if (Input.GetMouseButton(2))
        {
            var dir = previewOffset - GetViewPortCoords();
            var offset = GetOffset(transform, dir);
            transform.position += offset;
            target.position += offset;
            previewOffset = GetViewPortCoords();
        }


    }

    private Vector3 GetOffset(Transform target, Vector3 dir)
    {
        return dir.x * moveXSensivity * target.right + dir.y * moveYSensivity * target.up;
    }

    private void UpdateCamScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            var delta = Input.mouseScrollDelta.y * distanceDir * Time.deltaTime * 100;
            transform.Translate(delta * Vector3.back);
            currentDistanse += delta;
        }
    }

    private void UpdateCamPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            preview = GetViewPortCoords();
        }


        if (Input.GetMouseButton(0))
        {
            var dir = preview - GetViewPortCoords();
            transform.position = target.position;
            transform.Rotate(Vector3.right, dir.y * 180);
            transform.Rotate(Vector3.up, -dir.x * 180, Space.World);
            transform.Translate(currentDistanse * Vector3.back);
            preview = GetViewPortCoords();
        }
    }

    private Vector3 GetViewPortCoords()
    {
        return this.GetComponentInParent<Camera>().ScreenToViewportPoint(Input.mousePosition);
    }
}
